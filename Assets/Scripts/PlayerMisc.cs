using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMisc : MonoBehaviour
{
    public int health = 100;

    private Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        healthText = GameObject.FindGameObjectsWithTag("HealthText")[0].GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = $"Health: {health}";
        if (health <= 0)
        {
            print("DEAD");
        }
    }
}
