using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{

    void Awake()
    {
        LevelLoader levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();

        levelLoader.LoadNextLevel();
        
        // TEST 
        // levelLoader.LoadScene("Main");
    }
}
