using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeapon weapon;
    private GameObject Bullet;
    private GameObject Bullet_Emitter;
    private float Bullet_speed;
    private float range;

    [SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	void Start ()
	{
        Bullet = weapon.prefab;
        Bullet_Emitter = weapon.prefab_emitter;
        Bullet_speed = weapon.speed;
        range = 100f;

		if (cam == null)
		{
			Debug.LogError("PlayerShoot: No camera referenced!");
			this.enabled = false;
		}
	}

	void Update ()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
    }

    [Client]
    void Shoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                //Red Glow
            }
        }

        GameObject tempBullet = NetworkIdentity.Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
        tempBullet.GetComponent<Bullet>().setPlayerWhoShot(gameObject, weapon.damage, weapon.timer);

        Rigidbody tempBody;
        tempBody = tempBullet.GetComponent<Rigidbody>();
        //tempBody.AddForce(Bullet_Emitter.transform.forward * Bullet_speed);
        tempBody.AddForce(Bullet_Emitter.transform.forward * Bullet_speed);
	}

    [Client]
    public void hitPlayer(string _playerID, int _damage)
    {
        CmdPlayerShot(_playerID, _damage);
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {   
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
