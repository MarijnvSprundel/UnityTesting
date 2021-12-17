using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    private Text healthText;
    private RectTransform redTintTransform;
    private Vector2 lastScreenSize;
    private Vector2 newScreenSize;
    private GameObject deathUI;
    private GameObject gameUI;
    void Start()
    {
        deathUI = transform.Find("DeathUI").gameObject;
        gameUI = transform.Find("GameUI").gameObject;
        healthText = transform.Find("GameUI/HealthText").GetComponent<Text>();
        redTintTransform = transform.Find("DeathUI/RedTint").GetComponent<RectTransform>();
        lastScreenSize = new Vector2(Screen.width, Screen.height);
        newScreenSize = new Vector2(Screen.width, Screen.height);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        newScreenSize = new Vector2(Screen.width, Screen.height);
        if (lastScreenSize != newScreenSize)
        {
            UpdateUI();
            lastScreenSize = newScreenSize;
        }
        
    }

    private void UpdateUI()
    {
        redTintTransform.sizeDelta = newScreenSize;
        gameUI.GetComponent<RectTransform>().sizeDelta = newScreenSize;
        healthText.GetComponent<RectTransform>().anchoredPosition = new Vector2(newScreenSize.x / -3 + 200,  newScreenSize.y / -3);
    }
    public void UpdateHealth(int health)
    {
        healthText.text = $"Health: {health}";
    }

    public void Death()
    {
        deathUI.SetActive(true);
        gameUI.SetActive(false);
    }

    public void Respawn()
    {
        deathUI.SetActive(false);
        gameUI.SetActive(true);
    }
}
