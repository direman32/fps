using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeapon weapon;
    private GameObject Bullet;
    private GameObject Bullet_Emitter;
    private float Bullet_speed;
    private List<GameObject> bullets = new List<GameObject>();

    [SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	void Start ()
	{
        Bullet = weapon.prefab;
        Bullet_Emitter = weapon.prefab_emitter;
        Bullet_speed = weapon.speed;

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
	void Shoot ()
	{
        GameObject tempBullet;
        tempBullet = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
        bullets.Add(tempBullet);

        Rigidbody tempBody;
        tempBody = tempBullet.GetComponent<Rigidbody>();

        tempBody.AddForce(Bullet_Emitter.transform.forward * Bullet_speed);
       

		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask) )
		{
			if (_hit.collider.tag == PLAYER_TAG)
			{
                //Red Glow
                hitPlayer(_hit.collider.name, 10);

            }
		}

	}

    [Client]
    public void hitPlayer(string _playerID, int _damage)
    {
        CmdPlayerShot(_playerID, _damage);
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot.");
        
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
