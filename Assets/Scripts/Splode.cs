using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Splode : NetworkBehaviour
{
    public GameObject player;
    public GameObject particle;
    GameObject capsule;
    public int playerWeaponDamage;
    public float playerWeaponTimer;
    private const string PLAYER_TAG = "Player";
    List<GameObject> inExplody = new List<GameObject>();
    // private bool barrel;

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
        GameObject tempExp = NetworkIdentity.Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        Destroy(tempExp, 1f);
        if (collision.collider.tag == PLAYER_TAG)
        {
            print("BOOM");
        }
        explosion(gameObject);
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
    
    private void explosion(GameObject caller)
    {
        foreach (GameObject sploded in inExplody)
        {
            if (nameChecks(sploded.name))
            {
                if (sploded.tag == PLAYER_TAG)
                {
                    doDamage(sploded, 100);
                }

                explosionForces(sploded);
            }
            else if (!isThis(sploded))
            {
                Splode splode = sploded.GetComponent<Splode>();
                if (splode != null && !sploded.Equals(caller))
                    splode.explosion(gameObject);
            }
        }
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

    private void explosionForces(GameObject _sploded)
    {
        Vector3 pos = gameObject.transform.position;
        var heading = _sploded.transform.position - pos;
        var distance = heading.magnitude;
        var direction = heading / distance;
        _sploded.GetComponent<Rigidbody>().AddForce(heading * 500);
    }

    void CmdPlayerShot(GameObject player, int _damage)
    {
        player.GetComponent<PlayerShoot>().hitPlayer(player.name, _damage);
    }
}
