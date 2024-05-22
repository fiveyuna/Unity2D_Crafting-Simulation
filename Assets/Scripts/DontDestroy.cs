using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject headerUI;
    [SerializeField] private Toggle inventoryToggle;
    [SerializeField] private GameObject startButton;

    private bool isChangeBGM = false;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("dont destroy");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // For UI header button
    public void OnOffInventoryUI(bool isOn) {
        inventoryUI.SetActive(isOn);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // Debug.Log("Dont destroy in Scene : " + scene.name);
        headerUI.SetActive(true);
        OnOffInventoryUI(false);
        inventoryToggle.isOn = false;

        if (scene.name == "Craft" || scene.name == "Study" || scene.name == "Note"
            || scene.name == "Start" || scene.name == "Dialogue") {
            inventoryUI.SetActive(false);
            headerUI.SetActive(false);
        }

        if (scene.name == "Start") {
            startButton.SetActive(false);
        } else {
            startButton.SetActive(true);
        }

        if (scene.name == SceneName.Adv_main.ToString()) {
            AudioManager.Inst.PlayBGM(AudioManager.Inst.GetClipFromPlaylist(SoundName.BGM_Adventure.ToString()), MusicTransition.CrossFade);
            isChangeBGM = true;
        }
        else if (scene.name == SceneName.Study.ToString()) {
            AudioManager.Inst.PlayBGM(AudioManager.Inst.GetClipFromPlaylist(SoundName.BGM_Study.ToString()), MusicTransition.CrossFade);

            isChangeBGM = true;
        }
        else if (scene.name == SceneName.Dialogue.ToString()) {
            AudioManager.Inst.PlayBGM(AudioManager.Inst.GetClipFromPlaylist(SoundName.BGM_Dialogue.ToString()), MusicTransition.CrossFade);

            isChangeBGM = true;
        }
        else if (isChangeBGM) {
            AudioManager.Inst.PlayBGM(AudioManager.Inst.GetClipFromPlaylist(SoundName.BGM_Title.ToString()), MusicTransition.CrossFade);
            isChangeBGM = false;
        }
    }

    

}
