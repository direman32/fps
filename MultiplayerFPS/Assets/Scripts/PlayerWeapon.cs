using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(GameObject))]
public class PlayerWeapon {

	public string name = "Glock";

	public int damage = 10;
	public float range = 100f;
	public GameObject prefab;
	public GameObject prefab_emitter;
	public float speed;
}
