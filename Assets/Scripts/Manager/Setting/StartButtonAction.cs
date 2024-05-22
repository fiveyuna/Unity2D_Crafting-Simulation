using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonAction : MonoBehaviour
{
    // [SerializeField] private GameObject dontDestroyCanvas;

    public void ButtonEvent_Click()
    {
        LevelLoader levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        if (levelLoader != null) {
            levelLoader.LoadScene(SceneName.Start);
            // Destroy(dontDestroyCanvas);
        } else {
            Debug.LogError("There is no levelLoader in the scene. so Start Button is not working.");
        }
    }
}
