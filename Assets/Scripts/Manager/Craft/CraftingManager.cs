using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour, IDataPersistence
{
    [Header("UI")]
    public CraftSlot[] craftSlots;
    public Transform outputSlot;
    // [SerializeField] private GameObject pfInventoryItem;
    [SerializeField] private GameObject pfCreatedItem;
    [SerializeField] private Button getItemButton;
    [SerializeField] private Animator craftItemAnimator;
    [SerializeField] private GameObject slotCoverUI;

    [Header("Issue Response")]
    [HideInInspector] public ItemSO createdItem;

    [Header("Unlock")]
    [SerializeField] private GameObject[] unlockCheckUI;
    
    /// <summary>
    ///  Modify for backup
    /// </summary>
    // [SerializeField] private GameObject[] unlockBackgroundUI;
    private bool[] isCraftUnlocked;

    [Header("Game")]
    [SerializeField] private LevelLoader levelLoader;
    [HideInInspector] public int ingredientsCount = 1;
    private List<RecipeSO> recipeList;
    private InventoryManager inventoryManager;
    [HideInInspector] public static CraftingManager instance;
    [HideInInspector] public List<string> storyProgress;


    private void Awake() {
        instance = this;

        recipeList = Resources.LoadAll("SORecipes", typeof(RecipeSO)).Cast<RecipeSO>().ToList();

        getItemButton.gameObject.SetActive(false);

        ingredientsCount = 1;
    }

    private void Start() {
        // isCraftUnlocked==True : slot is unlocked.
        SetUnlockUI();
        OnOffSlotCovers(false);

        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    // private void OnDisable()
    // {
    //     Debug.Log("Ondisable craft");
    //      AddSlotItemData();
    // }

    // Button Onclick method
    public void TryCraft() 
    {
        // Already have output
        if (outputSlot.childCount > 0) 
        {
            GetCreatedItem(createdItem);
        }

        // Set ingredient list
        List<ItemSO> craftIngredients = new List<ItemSO>(); 
        foreach (CraftSlot slot in craftSlots) 
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) {
                craftIngredients.Add(itemInSlot.item);
            }
        }

        // If is not empty ingredient slot
        if (craftIngredients.Count > 1) 
        {
            // Check all recipe
            foreach (RecipeSO r in recipeList) 
            { 
                // Compare list's count
                if (craftIngredients.Count == r.ingredients.Count)
                { 
                    // Compare without order of list
                    bool isEqual = Enumerable.SequenceEqual(craftIngredients.OrderBy(x => x.itemId), r.ingredients.OrderBy(x => x.itemId));
                    
                    // Find Collect Recipe
                    if (isEqual) 
                    { 
                        // Craft Animation, Audio ETC
                        craftItemAnimator.Play("clock_in");
                        ParticlePlay.instance.PlayParticle(0);
                        AudioManager.Inst.PlayOneShot(SoundName.SFX_Craft);
                        
                        DeleteSlotItems(); // Delete used ingredients
                        
                        CraftNewItem(r.outputItem); // Create result item
                        
                        if (TutorialManager.tutoInstance != null 
                            && TutorialManager.tutoInstance.IsTutorialPlaying())
                        {
                            TutorialManager.tutoInstance.EnterDialogueMode();
                        }
                        return;
                    }
                }
            }
        }

    }

// Button event
    public void Button_RefreshCraftSlot() {
        // TODO : Cope with result-false when AddItem()
        foreach (CraftSlot slot in craftSlots) {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) {
                inventoryManager.AddItem(itemInSlot.item, ingredientsCount);
            }
        }
        DeleteSlotItems();

        ingredientsCount = 1;
        OnOffSlotCovers(false);
    }

    // 재료 + 1, 빈 슬롯 = 잠금
    public void Button_IsAddIngredients(bool isAdd) {
        List<InventoryItem> craftIngredients = new List<InventoryItem>(); 
        List<ItemSO> craftItems = new List<ItemSO>();

        foreach (CraftSlot slot in craftSlots) {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) {
                craftIngredients.Add(itemInSlot);
                craftItems.Add(itemInSlot.item);
            }
        }
        
        if (craftIngredients.Count < 2) { return; }

        if (isAdd) { // Add
            if (inventoryManager.IsAddIngredientItems(craftItems)) {
                ingredientsCount++;
                foreach (InventoryItem cItem in craftIngredients) {
                    cItem.count++;
                    cItem.RefreshCount();
                }
            } else {
                Debug.Log("[CraftM] Insufficient Ingredients.");
            }
            // // TODO : Refresh count of item -> Modify Refresh slot addItem (w.count), Craft result Item 
        } else if (ingredientsCount > 1) { // Subtract
            // // TODO : if ingCount == 0 : interact false?
            foreach (ItemSO item in craftItems) {
                // TODO : Cope with result-false when AddItem()
                inventoryManager.AddItem(item, 1);
            }

            ingredientsCount--;
            foreach (InventoryItem item in craftIngredients) {
                item.count--;
                item.RefreshCount();
            }
        }

        // // TODO : If count== unlock the locked craft slots (Cover with X image)
        if (ingredientsCount == 1) {
            OnOffSlotCovers(false);
        } else if (ingredientsCount == 2) {
            OnOffSlotCovers(true);
        }
        
    }

    private void OnOffSlotCovers(bool isOn) {
        slotCoverUI.SetActive(isOn);        
    }

    public void Button_CompleteResearch()
    {
        bool isComplete = true;
        foreach (bool isUnlocked in isCraftUnlocked) {
            if (!isUnlocked) {
                isComplete = false;
                break;
            }
        }
        
        if (isComplete) {
            storyProgress.Add("complete");
            AudioManager.Inst.PlayOneShot(SoundName.SFX_Craft);
            levelLoader.LoadScene(SceneName.Dialogue);
        } else {
            AudioManager.Inst.PlayOneShot(SoundName.SFX_Wrong);
        }
        
    }

// display to inventory
    private void CraftNewItem(ItemSO item) {
        // For add item when scene move
        createdItem = item;

        // Craft new item
        GameObject newItem = Instantiate(pfCreatedItem, outputSlot);
        newItem.GetComponent<Image>().sprite = item.image;
        newItem.GetComponentInChildren<TextMeshProUGUI>().text = (ingredientsCount==1) ? "" : ingredientsCount.ToString();
        
        // Set Get button
        getItemButton.gameObject.SetActive(true);

        getItemButton.onClick.RemoveAllListeners();
        getItemButton.onClick.AddListener(() => GetCreatedItem(item));
    }

    private void GetCreatedItem(ItemSO item) {

        craftItemAnimator.Play("clock_out");

        bool result = inventoryManager.AddItem(item, ingredientsCount);
        if (!result) {
            Debug.Log("[CraftM] Full inventory.");
            return;
        }

        Transform Created = outputSlot.GetChild(0);
        Destroy(Created.gameObject);
        getItemButton.gameObject.SetActive(false);

        ingredientsCount = 1;
    }

    private void DeleteSlotItems() {
        foreach (CraftSlot slot in craftSlots) {
            if (slot.GetComponentInChildren<InventoryItem>() != null) {
                GameObject itemInSlot = slot.transform.GetChild(0).gameObject;
                Destroy(itemInSlot);
            }
        }
    }

// Unlock functions
    public void UnlockTheSlot(int partId) {
        isCraftUnlocked[partId] = true;
        SetUnlockUI();
    }

    private void SetUnlockUI() {
        for(int i = 0; i<isCraftUnlocked.Length; i++) {
            unlockCheckUI[i].SetActive(isCraftUnlocked[i]);
            // unlockBackgroundUI[i].SetActive(isCraftUnlocked[i]);
        } 
    }

    // disable
    public void AddSlotItemData() {
        Debug.Log("[test] addslot");
        // add ouput slot item
        if (this.outputSlot.childCount > 0) {
            inventoryManager.AddItem(this.createdItem, this.ingredientsCount);
            Debug.Log("Test. AddSlotItemData() output item : " + this.createdItem.name + ", count : " + this.ingredientsCount);

        }

        // add input slot item
        foreach (CraftSlot slot in this.craftSlots) {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) {
                inventoryManager.AddItem(itemInSlot.item, this.ingredientsCount);
                Debug.Log("Test. AddSlotItemData() item : " + itemInSlot.item.name + ", count : " + this.ingredientsCount);
            }
        }

        inventoryManager.InventoryItemListToDataList();
    }

    public void ButtonEvent_LoadNote()
    {
        PlayerPrefs.SetString("BeforeNote", "Craft");
    }

// Data Persistence
    public void LoadData(GameData data)
    {
        this.isCraftUnlocked = data.isCraftUnlocked; 
        this.storyProgress = data.storyProgress;
    }

    public void SaveData(GameData data)
    {
        data.isCraftUnlocked = this.isCraftUnlocked;
        data.storyProgress = this.storyProgress;
    }
}
