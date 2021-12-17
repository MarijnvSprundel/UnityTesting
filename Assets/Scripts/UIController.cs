using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    private Text healthText;
    void Start()
    {
        healthText = transform.Find("GameUI").transform.Find("HealthText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHealth(int health)
    {
        healthText.text = $"Health: {health}";
    }

    public void Death()
    {
        
    }

    public void Respawn()
    {
        
    }
}
