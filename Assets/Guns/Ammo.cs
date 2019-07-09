using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ammo
{
    void setPlayerWhoShot(GameObject _player, int damage, float timer);
    void CmdPlayerShot(string _playerID, int _damage);
}
