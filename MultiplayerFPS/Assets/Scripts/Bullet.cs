using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public GameObject player;
    private const string PLAYER_TAG = "Player";
    void Start()
    {
        Destroy(gameObject, 3);
    }

    [Client]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == PLAYER_TAG)
        {
            CmdPlayerShot(collision.collider.name, 10);
        }
        Destroy(gameObject);
    }
   
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot.");

       // player.GetComponent<PlayerShoot>().hitPlayer(_playerID, _damage);
    }
}
