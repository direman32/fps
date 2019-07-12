using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Splode : NetworkBehaviour
{
    public GameObject player;
    public GameObject particle;
    public GameObject barrelParts;
    GameObject capsule;
    public int playerWeaponDamage;
    public float playerWeaponTimer;
    private static readonly string PLAYER_TAG = "Player";
    List<GameObject> inExplody = new List<GameObject>();

    void Start()
    {
        capsule = gameObject.transform.GetChild(0).gameObject;
    }

    [Client]
    private void doDamage(GameObject player, int damage)
    {
        CmdPlayerShot(player, damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Bullet")) {
            GameObject tempExp = NetworkIdentity.Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            Destroy(tempExp, 1f);
            explosion(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            inExplody.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        inExplody.Remove(other.gameObject);
    }
    
    public void explosion(GameObject caller)
    {
        foreach (GameObject sploded in inExplody)
        {
            if(sploded!= null) {
                if (nameChecks(sploded.name))
                {
                    if (sploded.tag == PLAYER_TAG)
                    {
                        doDamage(sploded, Mathf.RoundToInt(100 /explosionForces(sploded)));
                    }
                }
                else if (!isThis(sploded))
                {
                    Splode splode = sploded.GetComponent<Splode>();
                    if (splode != null && !sploded.Equals(caller))
                        splode.explosion(gameObject);
                }
            }
        }
        GameObject tempBarrelParts = NetworkIdentity.Instantiate(barrelParts, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        Destroy(tempBarrelParts, 2f);
        Destroy(gameObject);
    }

    private bool nameChecks(string Name)
    {
        if (Name == null || Name.Equals(name))
        {
            return true;
        }
        foreach(Transform child in GetComponentsInChildren<Transform>())
        {
            return true;
        }
        return false;
    }

    private bool isThis(GameObject obj)
    {
        if (obj.Equals(gameObject))
            return true;
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.Equals(obj))
                return true;
        }
        return false;
    }

    private float explosionForces(GameObject _sploded)
    {
        Vector3 pos = gameObject.transform.position;
        var heading = _sploded.transform.position - pos;
        var distance = heading.magnitude;
        var direction = heading / distance;
        _sploded.GetComponent<Rigidbody>().AddForce(direction * 500);
        return distance - .75f;
    }

    void CmdPlayerShot(GameObject player, int _damage)
    {
        player.GetComponent<PlayerShoot>().hitPlayer(player.name, _damage);
    }
}
