using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Rocket : NetworkBehaviour
{
    public GameObject player;
    public GameObject particle;
    GameObject capsule;
    public int playerWeaponDamage;
    public float playerWeaponTimer;
    private const string PLAYER_TAG = "Player";
    List<GameObject> inExplody = new List<GameObject>();

    void Start()
    {
        Destroy(gameObject, 5f);
        capsule = gameObject.transform.GetChild(0).gameObject;
    }

    [Client]
    private void doDamage(string playerID, int damage)
    {
        CmdPlayerShot(playerID, damage);
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
        if (other.gameObject.GetComponent<Rigidbody>() != null && 
            !other.gameObject.Equals(gameObject)
            && !other.gameObject.Equals(capsule))
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
        foreach (GameObject sploded in inExplody) {
            if (!sploded.name.Equals(gameObject.name) && !sploded.name.Equals(capsule.name))
            {
                if(sploded.tag == PLAYER_TAG)
                    doDamage(sploded.name, 100);

                explosionForces(sploded);
            }
            else if (sploded != gameObject)
            {
                Rocket r = sploded.GetComponent<Rocket>();
                if(r != null && !sploded.Equals(caller))
                    r.explosion(gameObject);
            }
        }
        Destroy(gameObject);
    }

    private void explosionForces(GameObject _sploded)
    {
        Vector3 pos = gameObject.transform.position;
        var heading = _sploded.transform.position - pos;
        var distance = heading.magnitude;
        var direction = heading / distance;
        _sploded.GetComponent<Rigidbody>().AddForce(heading * 500);

        //_sploded.GetComponent<Rigidbody>()
         //           .AddExplosionForce(200 * 2,_sploded.transform.position,20);
    }

    public void setPlayerWhoShot(GameObject _player, int damage, float timer)
    {
        player = _player;
        playerWeaponDamage = damage;
        playerWeaponTimer = timer;
    }

    void CmdPlayerShot(string _playerID, int _damage)
    {
        player.GetComponent<PlayerShoot>().hitPlayer(_playerID, _damage);
    }
}
