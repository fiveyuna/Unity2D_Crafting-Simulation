using System;
using System.Collections;
using Ink.Runtime;
using TMPro;
using UnityEngine;
// using UnityEngine.Localization;
// using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Params")]
    // 작을 수록 더 빠른 타이밍 스피드
    [SerializeField] private float typingSpeed = BASIC_TYPE_SPEED;
    private TextAsset inkJSON_tutorial;
    [SerializeField] private TextAsset tutorial_en;
    [SerializeField] private TextAsset tutorial_kr;
    
    [SerializeField] private ItemSO[] items;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Game")]
    [SerializeField] private Button touchAreaButton;

    private bool isButtonClicked = false;
    private int tutorialProgress = 0;
    
    public static TutorialManager tutoInstance;
    private Story currentStory;
    private bool dialogueIsPlaying;

    private bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;

    // [ const value ]
    private const float BASIC_TYPE_SPEED = 0.04f;

    private void Awake()
    {
        if (tutoInstance != null){
            Debug.LogWarning("Found more than one Tutorial Manager in the scene");
        }
        
        tutoInstance = this;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        SetTutorialLanguage();
    }
    
    public bool IsTutorialPlaying()
    {
        if (tutorialProgress > 0 && tutorialProgress < 9) {
            return true;
        } else {
            return false;
        }
    }

    // Start Tutorial
    public void SetTutorialDefault()
    {
        Debug.Log("[Test] SetTutorialDefault()");
        this.gameObject.SetActive(true);
        tutorialProgress = 0;

        SetTutorialLanguage();
    }

    public void SetTutorialLanguage()
    {
        int localIndex = PlayerPrefs.HasKey("Locale") ? PlayerPrefs.GetInt("Locale") : 1;
        // Debug.Log("locla : " + localIndex);
        if (localIndex == 0) { // en
            inkJSON_tutorial = tutorial_en;  
        } else {
            inkJSON_tutorial = tutorial_kr;
        }

        // Locale currentLocale = LocalizationSettings.SelectedLocale;
        // if (currentLocale == LocalizationSettings.AvailableLocales.Locales[0]) { // en
        //     inkJSON_tutorial = tutorial_en;
        // } else if (currentLocale == LocalizationSettings.AvailableLocales.Locales[1]) { // kr
        //     inkJSON_tutorial = tutorial_kr;
        // } else {
        //     Debug.Log("locale : error");
        // }
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    // Dialogue
    public void Button_TouchedDialogueArea()
    {
        if (!dialogueIsPlaying) 
        {
            return;
        }

        if (!canContinueToNextLine) {
            isButtonClicked = true;
        }

        // handle continuing to the next line in the dialogue when submit is pressed
        // NOTE: The 'currentStory.currentChoiecs.Count == 0' part was to fix a bug after the Youtube video was made
        if (canContinueToNextLine 
            && currentStory.currentChoices.Count == 0) {
            ContinueStory();
        }
       
    }

    public void EnterDialogueMode()
    {
        // ink file load
        currentStory = new Story(inkJSON_tutorial.text);

        string knotName = "tuto_" + tutorialProgress;
        currentStory.ChoosePathString(knotName);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue) {

            // Set text for the current dialogue line.
            if (displayLineCoroutine != null) {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
        } else {
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(String line)
    {
        dialogueText.text = "";

        // hide items while text is typing
        continueIcon.SetActive(false);

        canContinueToNextLine = false;
        isButtonClicked = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time.
        foreach (char letter in line.ToCharArray()) {
            if (isButtonClicked) {
                dialogueText.text = line.Replace('@', '\n');
                break;
            }

            if (letter == '<' || isAddingRichTextTag) {
                isAddingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>' ) {
                    isAddingRichTextTag = false;
                }
            } else if (letter == '@') {
                dialogueText.text += '\n';
            } else {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // actions to take after the entire line has finished displaying
        continueIcon.SetActive(true);
        // display choices, if any, for this dialogue line

        canContinueToNextLine = true;
    }

    private void ExitDialogueMode() 
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        TutorialProgress();
    }

    private void TutorialProgress()
    {
        Debug.Log("[Test] tutorialProgress : " + tutorialProgress);
        LevelLoader levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        tutorialProgress++;
        
        switch (tutorialProgress)
        {
            case 1:
                /// Delete for Backup_V2
                /*
                // Debug.Log("Tutorial Start.");
                levelLoader.LoadScene(SceneName.Study);
                // PlayNextProgress()

                // 이미 진단평가를 했다면 진단평가 스킵
                if (PlayerPrefs.HasKey("currentStatus")
                    && PlayerPrefs.GetString("currentStatus") == "LEARNING")
                {
                    tutorialProgress++;
                }
                */
                tutorialProgress = 5;
                levelLoader.LoadScene(SceneName.Craft);

                break;
            case 3:
                SetInteractableFalse();
                break;
            case 5:
                levelLoader.LoadScene(SceneName.Craft);
                break;
            case 8:
                foreach (ItemSO item in items) {
                    InventoryManager.inventoryInstance.AddItem(item, 1);
                }
                break;
            case 9:
                Debug.Log("Tutorial End.");
                CraftingManager.instance.storyProgress.Remove("tutorial");

                MoveSceneInCraft.moveSceneInCraft.AddSlotItemData();
                levelLoader.LoadScene(SceneName.Main);
                this.gameObject.SetActive(false);
                break;
            // case 10:
            //     Debug.Log("[Test] tutorial off");
            //     this.gameObject.SetActive(false);
            //     break;
            
        }    
        
    }

    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        // Debug.Log("[Test] Scene changed from " + previousScene.name + " to " + newScene.name);

        if (IsTutorialPlaying()) {
            SetInteractableFalse();
        }
        
        // Study Scene, Craft Scene
        if (tutorialProgress == 1 || tutorialProgress == 5 || tutorialProgress == 7) {
            EnterDialogueMode();
        }
    }

    private void SetInteractableFalse()
    {
        // Set interactable false
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Tutorial");
        foreach (GameObject obj in objectsWithTag) {
            // Debug.Log("[test] obj : " + obj.ToString());
            Button button = obj.GetComponent<Button>();
            if (button != null) {
                button.interactable = false;
            }
        }
    }

    public int GetTutorialProgress() {
        return tutorialProgress;
    }

    public void ControlProgress(int i)
    {
        tutorialProgress = i;
        // Debug.Log("progress : " + tutorialProgress);
    }
}