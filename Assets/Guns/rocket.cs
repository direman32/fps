using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class rocket : Bullet
{
    void Start()
    {
    }

    [Client]
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void setPlayerWhoShot(GameObject _player, int damage, float timer)
    {
        base.setPlayerWhoShot(_player, damage, timer);
    }
}
