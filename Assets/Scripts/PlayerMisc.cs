using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMisc : MonoBehaviour
{
    public int health = 100;
    private UIController ui;
    
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        ui.UpdateHealth(health);
        if (health <= 0)
        {
            StartCoroutine(GetComponent<PlayerController>().PlayerDeath());
        }
    }
}
