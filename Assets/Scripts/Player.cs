using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;
    [SyncVar]
    public bool paused;

    [SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;
    private bool spawned = false;
    private float timer = 0f;

    public void Setup ()
    {
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++)
		{
			wasEnabled[i] = disableOnDeath[i].enabled;
		}

        SetDefaults();
    }

    void Update ()
    {
    	if (!isLocalPlayer)
    		return;

    	if (Input.GetKeyDown(KeyCode.K))
    	{
    		RpcTakeDamage(99999);
    	}
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused)
                setPaused(true);
            else
                setPaused(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Death"))
            Die();
        if (collision.gameObject.name.Equals("Spawn"))
            Die();
    }

    [ClientRpc]
    public void RpcTakeDamage (int _amount)
    {
		if (isDead)
			return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

		if (currentHealth <= 0)
		{
			Die();
		}
    }

	private void Die()
	{
		isDead = true;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = false;

		StartCoroutine(Respawn());
	}

    private IEnumerator Respawn ()
	{
        yield return new WaitForSeconds(timer);
        timer = GameManager.instance.matchSettings.respawnTime;

        Transform _spawnPoint;

        SetDefaults();
        do
        {
            _spawnPoint = NetworkManager.singleton.GetStartPosition();
            print(_spawnPoint.gameObject.name);
        } while (_spawnPoint.gameObject.name.Equals("SpawnPoint"));

        transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
    }

    public void SetDefaults ()
    {
		isDead = false;

        currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;

        setPaused(false);
    }

    void setPaused(bool _paused)
    {
        if (!_paused)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
        Cursor.visible = _paused;
        paused = _paused;
    }
}
