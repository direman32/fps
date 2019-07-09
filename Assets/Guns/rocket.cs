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
        foreach (GameObject go in inExplody) {
            if (!go.name.Equals(gameObject.name) && !go.name.Equals(capsule.name))
            {
                if(go.tag == PLAYER_TAG)
                    doDamage(go.name, 100);
                var heading = go.transform.position - gameObject.transform.position;
                var distance = heading.magnitude;
                var direction = heading / distance;
                go.GetComponent<Rigidbody>().AddForce(heading * 500);
                //go.GetComponent<Rigidbody>().AddExplosionForce(200 * 2, go.transform.position, 20);
            }
            else if (go != gameObject)
            {
                Rocket r = go.GetComponent<Rocket>();
                if(r != null && !go.Equals(caller))
                    r.explosion(gameObject);
            }
        }
        Destroy(gameObject);
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
