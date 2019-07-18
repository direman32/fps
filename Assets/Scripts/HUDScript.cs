using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public GameObject health;
    public GameObject feul;
    private Slider healthBar;
    private Slider feulBar;
    private GameObject player;
    private Player playerScript;

    void Start()
    {
        healthBar = health.GetComponentInChildren<Slider>();
        feulBar = feul.GetComponentInChildren<Slider>();
        playerScript = GetComponentInParent<Player>();
        player = playerScript.gameObject;
    }
    
    void Update()
    {
        healthBar.value = playerScript.getHealth();
        feulBar.value = player.GetComponentInChildren<PlayerController>().getFeul();
    }
}
