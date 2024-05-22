using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject tutorialPopup;

    private bool isOnNextDay = false;

    [Header("Dont Destroy")]
    // private HeaderManager headerManager;
    private TutorialManager tutorialManager;
    
    [Header("Data")]
    private int day = -1;
    private List<string> storyProgress;


    private void Start()
    {
        // Get Component
        // calendarText  = GameObject.Find("CalendarText").GetComponent<TextMeshProUGUI>();
        // headerManager = GameObject.Find("HeaderManager").GetComponent<HeaderManager>();

        HeaderManager.GetInstance().SetCalendarText();
        // Debug.Log("Get Header : " + HeaderManager.GetInstance().day);

        // Tutorial Setting
        tutorialPopup.SetActive(false);
        tutorialManager = TutorialManager.tutoInstance;
        
        if (storyProgress.Contains("tutorial")) {
            tutorialPopup.SetActive(true);
        } else if (tutorialManager != null) { // tutorial 이 아닌데 manager가 켜져있을 경우
            tutorialManager.gameObject.SetActive(false);
        }
    }

    // Tutorial
    public void ButtonEvent_StartTutorial()
    {
        tutorialPopup.SetActive(false);
        TutorialManager.tutoInstance.SetTutorialDefault();
        TutorialManager.tutoInstance.EnterDialogueMode();
    }

    public void ButtonEvent_QuitTutorial()
    {
        // Debug.Log("[test] working quit tuto");
        storyProgress.Remove("tutorial");
        tutorialPopup.SetActive(false);
        if (tutorialManager != null) {
            tutorialManager.gameObject.SetActive(false);
        }
    }

    // Calendar
    public void OnNextDay()
    {
        nextButton.interactable = false;

        // 중복 클릭 방지
        if (HeaderManager.GetInstance().day >= 30) {
            LevelLoader levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
            levelLoader.LoadScene("Dialogue");
            return;
        } else {
            bool isDone = NextDayAnimation.GetInstance().PlayNextDayAnimation();
            if (isDone) {
                // Debug.Log("[test] return. main nextButton interactable true");
                nextButton.interactable = true;
            }

            HeaderManager.GetInstance().MoveToNextDay();
            isOnNextDay = true;
            HeaderManager.GetInstance().SetHeaderDataDefault();
        }        
    }

    // Move to Note
    public void LoadNoteFromMain()
    {
        PlayerPrefs.SetString("BeforeNote", "Main");
    }

    public void LoadData(GameData data)
    {
        this.day = data.day;
        this.storyProgress = data.storyProgress;
    }

    public void SaveData(GameData data)
    {
        // data.day = this.day;
        if (isOnNextDay) {
            data.ClearAdvDepth();
        }
        data.storyProgress = this.storyProgress;
    }
}
