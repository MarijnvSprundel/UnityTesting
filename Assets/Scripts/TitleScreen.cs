using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    private Button playButton;

    void Start()
    {
        playButton = transform.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(Play);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Play()
    {
        SceneManager.LoadScene("TestGrounds");
    }
}
