using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class AdventureManager : MonoBehaviour, IDataPersistence
{
    [Header("Adventure Manager")]
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] backgroundSprites;

    [Header("Depth-Unlock")]
    [SerializeField] private Slider depthSlider;
    [SerializeField] private GameObject unlockObject;
    [HideInInspector] public ItemSO unlockItem;
    private Image slotImage;
    // private UnlockSlot unlockSlot;

    [Header("Forest Items")]
    [SerializeField] private ItemSO[] itemsInForest; // 불, 나뭇가지, 꽃, 사탕수수, 통나무, 마나초
    [SerializeField] private ItemSO[] unlocksInForest;

    [Header("Sea Items")]
    [SerializeField] private ItemSO[] itemsInSea; // 모래, 바닷물, 조개껍데기, 얼음, 해초, 소금, 마나결정
    [SerializeField] private ItemSO[] unlocksInSea;

    [Header("Mine Items")]
    [SerializeField] private ItemSO[] itemsInMine; // 구리광석, 돌, 기름, 불, 철광석, 금광석
    [SerializeField] private ItemSO[] unlocksInMine;
    
    [Header("Event 운디네-차분,운디네-활발,두더지,쮝")]
    [SerializeField] private ItemSO[] unlockEventItems; 
    [SerializeField] private ItemSO[] giftEventItems;

    // [Header("Dialogue Trigger Test")]
    // [SerializeField] private TextAsset inkJSON;

    [Header("Common Setting")]
    [HideInInspector] public List<string> storyProgress;
    private int depth = 0;
    private int advIndex = 0; // 0 : 숲, 1 : 겨울바다, 2 : 광산
    private string advName;
    private ItemSO[] unlockItems;
    private ItemSO[] itemsToSpawn;
    private bool isLocked = false;
    private bool isDialoguePlaying = false;
    private string eventName = "";

    [Header("Manager")]
    private InventoryManager inventoryManager;
    private HeaderManager headerManager;
    private static AdventureManager instance;

    [Header("Const value")]
    private const string ADV_FOREST = "forest";
    private const string ADV_SEA = "sea";
    private const string ADV_MINE = "mine";

    private void Awake()
    {
        if (PlayerPrefs.HasKey("AdventureIndex")) {
            advIndex = PlayerPrefs.GetInt("AdventureIndex");
        } else {
            Debug.LogError("[AdventureManager] There is no adventure key.");
        }

        if (instance != null) {
            Debug.LogWarning("Found more than one Adventure Manager in the scene");
        }
        
        instance = this;
    }

    public static AdventureManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {         
        // Set Get Item Line
        itemText.text = "";
        itemImage.gameObject.SetActive(true);
        Color c = itemImage.color;
        c.a = 0;
        itemImage.color = c;

        // Set Managers
        headerManager = GameObject.Find("HeaderManager").GetComponent<HeaderManager>();
        inventoryManager  = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        // Set Unlock Objs
        unlockObject.gameObject.SetActive(false);
        slotImage = unlockObject.transform.GetChild(1).GetComponent<Image>();

        // Set Common Setting
        backgroundImage.sprite = backgroundSprites[advIndex];
        
        switch (advIndex) {
            case 0: // forest
                unlockItems = unlocksInForest;
                itemsToSpawn = itemsInForest;
                advName = ADV_FOREST;
                break;
            case 1: // sea
                unlockItems = unlocksInSea;
                itemsToSpawn = itemsInSea;
                advName = ADV_SEA;
                break;
            case 2:
                unlockItems = unlocksInMine;
                itemsToSpawn = itemsInMine;
                advName = ADV_MINE;
                break;
            default:
                Debug.LogError("[AdventureM] There is wrong advIndex : " + advIndex);
                break;
        }

        // TODO : DEPTH ISSUE CHECK HARD
        if (depth == 30 || storyProgress.Contains("mouse")) {
        } else if (depth == 0 || depth == 10 || depth == 20) {       
        } else {
            isDialoguePlaying = true;
        }
        
    }

    // Button Utility
    public void TouchAdventureArea() {
        // Dialogue 진입 코드

        StopAllCoroutines();

        Color c = itemText.color;
    
        // Can continue
        if (!isDialoguePlaying) {
            string jsonName = "";
            
            if (depth == 30 || storyProgress.Contains("mouse")) {
                jsonName = "monster";
            } else if (depth == 0 || depth == 10 || depth == 20) {       
                jsonName = "mouse";
            } 

            if (jsonName != "") {
                c.a = 0;
                itemText.color = c;
                itemImage.color = c;

                DialogueTrigger.GetInstance().TriggerDialogue(jsonName, advName, depth);

                isDialoguePlaying = true;
                eventName = "";
            } else {
                Debug.LogError("Dialogue playing error. json name is null. depth = " + depth);
            }
            
        } else {
            c.a = 1;
            itemText.color = c;
            itemImage.color = c;

            int localIndex = PlayerPrefs.HasKey("Locale") ? PlayerPrefs.GetInt("Locale") : 1; // 0:en, 1:kr

            Locale currentLocale = LocalizationSettings.SelectedLocale;
            // string language = "kr";
            // if (currentLocale == LocalizationSettings.AvailableLocales.Locales[0]) { // en
            //     language = "en";
            // }

            // Check can continue adventure 
            if (depth >= 30) {
                itemText.text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "Adv-EndLoad", currentLocale);
                // itemText.text = "막다른 길에 도달했다.";
                itemImage.gameObject.SetActive(false);
                return;
            } else if (headerManager.energy <= 0) { // 사용할 수 있는 에너지가 없는 경우
                itemText.text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "Adv-NoEnergy", currentLocale);
                // itemText.text = "에너지가 없어 더 이상 갈 수 없다.";
                itemImage.gameObject.SetActive(false);
                return;
            }

            headerManager.energy--;
            headerManager.SetEnergyUI();

            if (!isLocked) {
                depth++;
                depthSlider.value = depth;

                if (depth == 10 || depth == 20 || depth == 30) {
                    isDialoguePlaying = false;
                }
            }

            int randId = GetItemId(depth);
            bool result = inventoryManager.AddItem(itemsToSpawn[randId], 1);

            if (result) {
                AudioManager.Inst.PlayOneShot(SoundName.SFX_Go);
                // Debug.Log("들감 : " + itemsToSpawn[randId].itemName);
                string getStr = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "Adv-GetText", currentLocale);
                // itemText.text = itemsToSpawn[randId].itemName + "을(를) 획득했다!";
                string itemStr = localIndex == 1 ? itemsToSpawn[randId].itemName : itemsToSpawn[randId].itemName_en;
                itemText.text = itemStr + getStr;

                
                itemImage.sprite = itemsToSpawn[randId].image;
            } else {
                // Debug.Log("안 들감");
                itemText.text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "Adv-FullInven", currentLocale);
                // itemText.text = "인벤토리가 가득 찼습니다.";

            }

            StartCoroutine(FadeOutText());
        }
    }

    // Control from dialogue
    public void DisplayOnUnlockSlot()
    {
        // if (depth == 10 || depth == 20) { // 깊이 == 10 or 20
        isLocked = true;
        unlockObject.gameObject.SetActive(true);

        int unlockIndex = (depth==10) ? 0 : 1;
        unlockItem = unlockItems[unlockIndex];
        slotImage.sprite = unlockItems[unlockIndex].image;
    }

    public void DisplayOffUnlockSlot()
    {
        isLocked = true;
        unlockObject.gameObject.SetActive(false);
    }

    public void DisplayOnEventUnlockSlot(int unlockIndex)
    {
        isLocked = true;
        unlockObject.gameObject.SetActive(true);

        unlockItem = unlockEventItems[unlockIndex];
        slotImage.sprite = unlockEventItems[unlockIndex].image;

        // 운디네-차분,운디네-활발,두더지,쮝
        switch(unlockIndex) {
            case 0:
                eventName = "undine_good"; break;
            case 1:
                eventName = "undine_bad"; break;
            case 2:
                eventName = "mole"; break;
            case 3:
                eventName = "mouse"; break;
            default:
                Debug.LogError("Wrong unlock Index : " + unlockIndex);
                break;
        }
        
    }

    // 운디네-차분,운디네-활발,두더지,쮝
    public void GetGiftItem(int index)
    {
        if (index != 3 && index != 4) {
            bool result = inventoryManager.AddItem(giftEventItems[index], 1);

            if (result) {
                Debug.Log("들감 : " + giftEventItems[index].itemName);
            } else {
                Debug.LogError("이벤트 아이템 안 들어감.");
                // TODO : 대응 필요
            }
        }
        
        if (index != 2) {
            string key = "";
            switch (index) {
                case 0:
                    key = "honey"; break;
                case 1:
                    key = "poison"; break;
                case 3:
                    key = "mouse"; break;
                case 4:
                    key = "stranger"; break;
            }

            if (!storyProgress.Contains(key)) {
                storyProgress.Add(key);
            }
        }
    } 

    private int GetItemId(int depth) {
        int step;
        if (depth <= 10) step = 1;
        else if (depth <= 20) step = 2;
        else step = 3;

        int resultId = 0;
        switch (advIndex) {
            case 0:
                resultId = GetItemIdInArea(step); break;
            case 1:
                resultId = GetItemIdInArea(step); break;
            case 2:
                resultId = GetItemIdInArea(step); break;
            default:
                Debug.LogError("[AdventureM] AdvIndex is wrong number. : " + advIndex); break;
        }
        
        return resultId;
    }
    
    private int GetItemIdInArea(int step) {
        int[] percentages = {};

        if (advIndex == 0) {
            switch (step) { // 불, 꽃, 나뭇가지, 사탕수수, 통나무, 마나초
                case 1:
                    percentages = new int[] {3, 2, 2}; break;
                case 2:
                    percentages = new int[] {1, 1, 1, 2, 2}; break;
                case 3:
                    percentages = new int[] {0, 1, 0, 2, 2, 3}; break;
            }
        } else if (advIndex == 1) { // 모래, 바닷물, 조개껍데기, 얼음, 해초, 소금, 마나결정
            switch (step) {
                case 1:
                    percentages = new int[] {3, 1, 2}; break;
                case 2:
                    percentages = new int[] {1, 0, 1, 1, 2, 1}; break;
                case 3:
                    percentages = new int[] {2, 0, 1, 1, 1, 1, 5}; break;
            }
        } else {
            switch (step) { // 구리광석, 돌, 기름, 불, 철광석, 금광석
                case 1:
                    percentages = new int[] {3, 1, 2, 3}; break;
                case 2:
                    percentages = new int[] {2, 0, 1, 1, 3}; break;
                case 3:
                    percentages = new int[] {0, 0, 0, 1, 1, 1}; break;
            }
        }
        

        float random = Random.value;
        float numForAdding = 0;
        float total = 0;
        
        for (int i = 0; i < percentages.Length; i++) {
            total += percentages[i];
        }

        for (int i = 0; i < percentages.Length; i++) {
            if (percentages[i] / total + numForAdding >= random) {
                return i;
            } else {
                numForAdding += percentages[i] / total;
            }
        }
        return 0;
    }

    private IEnumerator FadeOutText() {
        for (float i = 1f; i >= -0.1f; i -= 0.1f) {
            Color c = itemText.color;
            c.a = i;
            itemText.color = c;
            itemImage.color = c;
            yield return new WaitForSeconds(.1f);
        }
    }

    // Interact with UnlockSlot.cs
    public void UnlockTheSlot() {
        unlockObject.gameObject.SetActive(false);
        // depth++;
        isLocked = false;

        DialogueManager.GetInstance().MakeChoice(-1);
        if (eventName != "") {
            DialogueManager.GetInstance().ChooseKnotPathString("answer_" + eventName);
            Debug.Log("[ADV] Unlock event slot");
        } else {
            DialogueManager.GetInstance().ChooseKnotPathString("use_stuff");
            Debug.Log("[ADV] Unlock common slot");
        }
        
    }

    // Data Managing
    public void LoadData(GameData data)
    {
        depth = data.advDepth[advIndex];

        // Set depth value from data
        depthSlider.value = depth;

        this.storyProgress = data.storyProgress;
    }

    public void SaveData(GameData data)
    {
        data.advDepth[advIndex] = depth;

        data.storyProgress = this.storyProgress;
    }
}
