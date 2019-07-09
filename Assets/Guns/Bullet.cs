using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public GameObject player;
    public int playerWeaponDamage;
    public float playerWeaponTimer;
    private const string PLAYER_TAG = "Player";

    void Start()
    {
        Destroy(gameObject, playerWeaponTimer);
    }

    [Client]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == PLAYER_TAG)
        {
            CmdPlayerShot(collision.collider.name, playerWeaponDamage);
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
        print(playerWeaponDamage + " " + playerWeaponTimer);
        player.GetComponent<PlayerShoot>().hitPlayer(_playerID, _damage);
    }
}
