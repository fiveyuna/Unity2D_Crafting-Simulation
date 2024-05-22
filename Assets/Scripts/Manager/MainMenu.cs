using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startNoDataButton;
    [SerializeField] private Button startExistDataButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private Button[] buttons;

    private LevelLoader levelLoader;

    private void Start()
    {
        bool result = DataPersistenceManager.dataInstance.CheckIsGameDataExist();
        // Debug.Log("Main Result : " + result);
        continueButton.interactable = result;
        continueText.alpha = result ? 1f : 0.5f;

        // When Data Exist
        if (result) {
            startNoDataButton.gameObject.SetActive(false);
            startExistDataButton.gameObject.SetActive(true);
        } else {
            startNoDataButton.gameObject.SetActive(true);
            startExistDataButton.gameObject.SetActive(false);
        }

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        // AudioManager.Inst.PlayBGM(AudioManager.Inst.GetClipFromPlaylist(SoundName.BGM_Title.ToString()));
        
        // Tutorial setting
        // if (TutorialManager.tutoInstance != null) {
        //     TutorialManager.tutoInstance.SetTutorialDefault();
        // }
    }

    public void OnNewGameClicked()
    {
        DisableMenuButton();

        DataPersistenceManager.dataInstance.NewGame();
        // DataPersistenceManager.dataInstance.NewGameWithFileDelete();
        
        levelLoader.LoadScene("Dialogue");
        // SceneManager.LoadSceneAsync("Dialogue");
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButton();
        // SceneManager.LoadSceneAsync("Main");
        levelLoader.LoadNextLevel();
    }

    private void DisableMenuButton()
    {
        foreach (var button in buttons) {
            button.interactable = false;
        }
    }
}
