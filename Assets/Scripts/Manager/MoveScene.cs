using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void LoadNextScene(string sceneName) {
        if (sceneName.Equals("Note")){
            PlayerPrefs.SetString("BeforeNote", "Main");
        }
        SceneManager.LoadScene(sceneName);
    }

    public void LoadAdventureScene(int advIndex) {
        PlayerPrefs.SetInt("AdventureIndex", advIndex);
        SceneManager.LoadScene("Adv_main");
    }
}
