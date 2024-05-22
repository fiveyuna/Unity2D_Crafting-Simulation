using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestCollectRecipe2 : MonoBehaviour, IDataPersistence
{
    [SerializeField] private ItemSO[] eventItems;
    [SerializeField] private ItemSO[] researchItems;
    [SerializeField] private ItemSO[] undineItems;

    private List<string> recipesCollected;
    private List<RecipeSO> allRecipeList;
    private List<string> storyProgress;

    private DataPersistenceManager dataPersistenceManager;
    private InventoryManager inventoryManager;


    private void Awake() {
        allRecipeList = Resources.LoadAll("SORecipes", typeof(RecipeSO)).Cast<RecipeSO>().ToList();
        dataPersistenceManager = GameObject.FindWithTag("dataPersistence").GetComponent<DataPersistenceManager>();

        inventoryManager  = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }
    
    

    // button utility
    public void Button_CollectTestRecipe(string key) 
    {
        if (!recipesCollected.Contains(key)){ 
            recipesCollected.Add(key);
        } else {
            Debug.Log("This key is already exist. key : " + key);
        }
    }

    public void Button_CollectAllRecipe()
    {
        Debug.Log("[Test] Collect All Recipe");
        foreach (var recipe in allRecipeList) {
            string key = recipe.type.ToString() + "-" + recipe.id;
            if (!recipesCollected.Contains(key)){ 
                recipesCollected.Add(key);
            } else {
                Debug.Log("This key is already exist. key : " + key);
            }
        }
    }

    public void Button_CheckDuplicateRecipe() {
        for (int i = 0; i < allRecipeList.Count; i++) {
            for (int j = i+1; j < allRecipeList.Count; j++) {
                if (allRecipeList[j].ingredients.Count == allRecipeList[i].ingredients.Count) { // Check equal count
                    // compare without order of list
                    bool isEqual = Enumerable.SequenceEqual(allRecipeList[j].ingredients.OrderBy(x => x.itemId),allRecipeList[i].ingredients.OrderBy(x => x.itemId));
                    // Debug.Log("[Craft] isEqual : " + isEqual);
                    if (isEqual) {
                        Debug.Log("Equal a : " + allRecipeList[i].outputItem.name + ", b : " + allRecipeList[j].outputItem.name);
                    }
                }
            }
        }

        Debug.Log("Check recipe done");
    }

    public void Button_CollectEventItems()
    {
        foreach (ItemSO item in eventItems) {
            bool result = inventoryManager.AddItem(item, 1);

            if (!result) {
                Debug.LogWarning("안 들감");
            }
        }

        Debug.Log("이벤트 아이템 다 들감");
        
    }

    public void Button_CollectEndItems()
    {
        foreach (ItemSO item in researchItems) {
            bool result = inventoryManager.AddItem(item, 1);

            if (!result) {
                Debug.LogWarning("안 들감");
            }
        }

        Debug.Log("연구 아이템 다 들감");
    }

    public void Button_CollectUndineItems()
    {
        storyProgress.Add("honey");
        storyProgress.Add("poison");
        foreach (ItemSO item in undineItems) {
            bool result = inventoryManager.AddItem(item, 1);

            if (!result) {
                Debug.LogWarning("안 들감");
            }
        }

        Debug.Log("엔딩 조건 활성화 아이템 다 들감");
        
    }

    public void Button_ClearStudyData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("All PlayerPrefs data has been cleared.");
    }

    public void LoadData(GameData data)
    { 
        this.recipesCollected = data.recipesCollected;
        this.storyProgress = data.storyProgress;
        
        /// Active function for backup
        Button_CollectAllRecipe();
    }

    public void SaveData(GameData data)
    {
        data.recipesCollected = this.recipesCollected;
        data.storyProgress = this.storyProgress;
    }
}
