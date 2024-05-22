using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
// using UnityEngine.Localization.Settings;
// using UnityEngine.Localization;

public class DialogueManager_Diary : MonoBehaviour, IDataPersistence
{
    [Header("Params")]
    // 작을 수록 더 빠른 타이밍 스피드
    [SerializeField] private float typingSpeed = BASIC_TYPE_SPEED;
    
    [Header("Pf Dialogue")]
    [SerializeField] private GameObject pfDialogue;
    [SerializeField] private GameObject dialogueParent;
    [SerializeField] private Sprite[] introImages;
    [SerializeField] private Sprite[] endingImages;

    [Header("Dialogue UI")]
    // [SerializeField] private Button touchAreaButton;
    private PrefabDialogue prefabDialogue;
    // private TextMeshProUGUI dialogueText;
    // private TextMeshProUGUI nameText;
    // private GameObject nameFrame;
    // private Image image;

    // [Header("Choice UI")]
    // private GameObject[] choices;
    // private TextMeshProUGUI[] choicesText;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON_intro_kr;
    [SerializeField] private TextAsset inkJSON_intro_en;
    [SerializeField] private TextAsset inkJSON_ending_kr;
    [SerializeField] private TextAsset inkJSON_ending_en;

    [Header("Game")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button endButton;
    private bool isButtonClicked = false;
    private List<string> storyProgress;
    private Sprite[] currentImages;
    private bool isEnding = true;

    private static DialogueManager_Diary instance;
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;

    private LevelLoader levelLoader;

    // [ const value ]
    private const float BASIC_TYPE_SPEED = 0.04f;
    private const string NAME_TAG = "name";
    private const string IMAGE_TAG = "img";


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }

        instance = this;
        dialogueIsPlaying = false;

        storyProgress = new List<string>();
        // dialogueVariables = new DialogueVariables(loadGlobalsJSON);
    }

    private void Start() {

        int localIndex = PlayerPrefs.HasKey("Locale") ? PlayerPrefs.GetInt("Locale") : 1;
        // Debug.Log("[Test] local : " + localIndex);
        if (localIndex == 0) { // en
            
        } else {
            
        }

        // Locale currentLocale = LocalizationSettings.SelectedLocale;

        // int localIndex = 1; // kr
        // if (currentLocale == LocalizationSettings.AvailableLocales.Locales[0]) { // en
        //     localIndex = 0;
        // }

        if (storyProgress.Contains("intro")) {
            currentImages = introImages;
            isEnding = false;
            storyProgress.Remove("intro");

            TextAsset inkJSON_dialogue = localIndex == 1 ? inkJSON_intro_kr : inkJSON_intro_en;
            EnterDialogueMode(inkJSON_dialogue);
        } 
        else {
            currentImages = endingImages;
            TextAsset inkJSON_dialogue = localIndex == 1 ? inkJSON_ending_kr : inkJSON_ending_en;
            EnterDialogueMode(inkJSON_dialogue);
        }

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

    }

    public static DialogueManager_Diary GetInstance()
    {
        return instance;
    }

    // Put function to touchArea in dialogue
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

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        // ink file load
        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;
        endButton.interactable = false;

        // 엔딩 - 연구도구 완성 유무, 갖고 있는 물건 유무
        if (isEnding) {
            // 연구도구 완성 실패
            if (!storyProgress.Contains("complete")) {
                ChooseKnotPathString("ending_0");
            }
            // 연구도구 완성
            else {
                // 선택지 조건 달성 여부 확인
                currentStory.variablesState["honey"] = storyProgress.Contains("honey");
                currentStory.variablesState["poison"] = storyProgress.Contains("poison");
            }
        }

        ContinueStory();
    }

    public void ChooseKnotPathString(string knotName)
    {
        currentStory.ChoosePathString(knotName);
    }

    // 나가가 버튼 클릭 시 또 못 누르게 설정
    public void Button_FalseInteractableControl()
    {
        endButton.interactable = false;
    }

    /**
    *
    * 마우스 클릭 -> 다음 스크립트 실행
    * 다음 스크립트 없음 -> ExitDialogueMode()
    *
    **/
    private void ContinueStory()
    {
        if (currentStory.canContinue) {
            // Set text for the current dialogue line.
            if (displayLineCoroutine != null) {
                StopCoroutine(displayLineCoroutine);
            }

            SpawnNewDialogue();

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            // handle tags
            HandleTags(currentStory.currentTags);
           
        } else {
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(String line)
    {
        // dialogueText.text = "";
        prefabDialogue.lineText.text = "";

        // hide items while text is typing
        HideChoices();
    
        if (currentStory.currentChoices.Count > 0) {
            prefabDialogue.choiceParent.SetActive(true);
        }

        canContinueToNextLine = false;
        isButtonClicked = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time.
        foreach (char letter in line.ToCharArray()) {
            if (isButtonClicked) {
                // dialogueText.text = line.Replace('@', '\n');
                prefabDialogue.lineText.text = line.Replace('@', '\n');
                break;
            }

            if (letter == '<' || isAddingRichTextTag) {
                isAddingRichTextTag = true;
                // dialogueText.text += letter;
                prefabDialogue.lineText.text += letter;
                if (letter == '>' ) {
                    isAddingRichTextTag = false;
                }
            } else if (letter == '@') {
                // dialogueText.text += '\n';
                prefabDialogue.lineText.text += '\n';
            } else {
                prefabDialogue.lineText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // display choices, if any, for this dialogue line
        DisplayChoices();

        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        // prefabDialogue.choiceParent.SetActive(false);
        foreach (GameObject choiceButton in prefabDialogue.choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags) 
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) {
                Debug.LogError("[DialogueManager] Tag 가 잘못 기록되었습니다. : " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey) {
                case NAME_TAG:
                    prefabDialogue.nameFrame.SetActive(true);
                    prefabDialogue.nameText.text = tagValue;
                    break;
                case IMAGE_TAG:
                    prefabDialogue.image.gameObject.SetActive(true);
                    prefabDialogue.image.sprite = currentImages[int.Parse(tagValue)];
                    break;
                default:
                    Debug.LogWarning("해당 Tag 는 존재하지 않습니다. : " + tagKey);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > prefabDialogue.choices.Length) {
            Debug.LogError("설정된 선택지보다 더 많은 선택지가 작성되었습니다. 현재 선택지 수 : " + currentChoices.Count);
        }

        // When Choice on
        if (currentChoices.Count > 0) {
            nextButton.interactable = false;
        }

        int index = 0;
        foreach (Choice choice in currentChoices) {
            prefabDialogue.choices[index].gameObject.SetActive(true);
            prefabDialogue.choicesText[index].text = choice.text;
            index++;
        }

        for (int i=index; i < prefabDialogue.choices.Length; i++) {
            prefabDialogue.choices[i].gameObject.SetActive(false);
        }
    }
    
    // 대화 끝
    private void ExitDialogueMode() 
    {
        dialogueIsPlaying = false;

        endButton.interactable = true;
        nextButton.interactable = false;

        if (isEnding) {
            endButton.onClick.AddListener(() => levelLoader.LoadScene("Start"));
        } else {
            endButton.onClick.AddListener(() => levelLoader.LoadScene("Main"));
        }
    }

    // Click choice button / -1 : from external function
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine) {
            // touchAreaButton.gameObject.SetActive(true);
            nextButton.interactable = true;
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }

    }

    private void SpawnNewDialogue() {
        GameObject newDialogue =  Instantiate(pfDialogue, dialogueParent.transform);
        prefabDialogue = newDialogue.GetComponent<PrefabDialogue>();

        // UIWidget uiWidget= newDialogue.GetComponent<UIWidget>();
        // uiWidget.Show();
        // prefabDialogue 

        // newItem.gameObject.name = newItem.gameObject.name.Replace("(Clone)","").Trim();
        // InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        // inventoryItem.count = count;

        // inventoryItem.InitializeItem(item);
        // inventoryItem.RefreshCount();
    }

    public void LoadData(GameData data)
    {
        this.storyProgress = data.storyProgress;
    }

    public void SaveData(GameData data)
    {
        // Not Use
    }
}
