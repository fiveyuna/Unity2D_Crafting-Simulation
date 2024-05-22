using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DevionGames.UIWidgets;
// using UnityEngine.Localization;
// using UnityEngine.Localization.Settings;

public class NoteManager : MonoBehaviour, IDataPersistence
{
    [Header("UI")]
    // [SerializeField] private List<RecipeSO> recipes;
    [SerializeField] private GameObject recipeListUI;
    // [SerializeField] private GameObject recipeDetailUI;
    [SerializeField] private TextMeshProUGUI recipeTypeText;
    // private GameObject dontDestroyCanvas;

    [Header("GAME")]
    [SerializeField] private GameObject pfRecipeBtn;
    [SerializeField] private GameObject pfRecipeNone;
    [SerializeField] private RecipeSO notCollectedRecipe;

    [Header("DETAIL")]
    [SerializeField] private TextMeshProUGUI recipeIdText;
    [SerializeField] private Image recipeImage;
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Image[] ingredientsImage;
    [SerializeField] private Sprite defaultSlotImage;
    [SerializeField] private TextMeshProUGUI recipeDesc;
    [SerializeField] private TextMeshProUGUI recipeItemDesc;
    [SerializeField] private WidgetTrigger widgetTrigger;

    [Header("DATA")]
    private RecipeType currentType;
    private List<string> recipesCollected;
    
    [Header("ETC")]
    private List<RecipeSO> recipes;
    private const int MAX_INGREDIENT = 4;
    
    private void Awake() {
        // dontDestroyCanvas = GameObject.FindWithTag("dont destroy");
        // dontDestroyCanvas.SetActive(false);

        recipes = Resources.LoadAll("SORecipes", typeof(RecipeSO)).Cast<RecipeSO>().ToList();
        recipes = recipes.OrderBy(x => x.id).ToList(); // id 순 정렬
        

        recipesCollected = new List<string>();

        currentType = RecipeType.Basic;

    }

    private void Start()
    {
        recipeListUI.SetActive(true);
        // recipeDetailUI.SetActive(false);

        SettingRecipeList(currentType);
    }

    private void SettingRecipeList(RecipeType type) {
        // Debug.Log("=== type: " + type.ToString() + "===");
        // int i = 0;
        int localIndex = PlayerPrefs.HasKey("Locale") ? PlayerPrefs.GetInt("Locale") : 1;
        // Debug.Log("locla : " + localIndex);
        if (localIndex == 0) { // en
            recipeTypeText.text = "< " + RecipeNameStr.recipeName_en[type] + " >";
        } else {
            recipeTypeText.text = "< " + RecipeNameStr.recipeName_kr[type] + " >";
        }

        foreach (RecipeSO recipe in recipes) {
            if (recipe.type == type) {
                // Debug.Log(i++ + ". " + recipe.outputItem.itemName);
                if (recipesCollected.Contains(type +"-" + recipe.id)) { // true : Collected Recipe
                    SpawnNoteRecipe(recipe);
                } else { // false : Not Collected Recipe
                    SpawnNoteRecipe(notCollectedRecipe);
                }
                
            }
        }
    }

    private void SettingRecipeDetail(RecipeSO recipe) {
        recipeIdText.text = "" + recipe.id + ".";
        recipeImage.sprite = recipe.outputItem.image;

        int localIndex = PlayerPrefs.HasKey("Locale") ? PlayerPrefs.GetInt("Locale") : 1;
        
         string descStr = "";
        if (localIndex == 0) {
            recipeName.text = recipe.outputItem.itemName_en;
            recipeItemDesc.text = recipe.outputItem.itemDesc_en;
            foreach (ItemSO ig in recipe.ingredients) {
                descStr += ig.itemName_en + " + ";
            }
        } else {
            recipeName.text = recipe.outputItem.itemName;
            recipeItemDesc.text = recipe.outputItem.itemDesc;
            foreach (ItemSO ig in recipe.ingredients) {
                descStr += ig.itemName + " + ";
            }
        }
        descStr = descStr.Substring(0, descStr.Length - 3);
        recipeDesc.text = descStr;

        int i=0;
        for (; i<recipe.ingredients.Count; i++) {
            ingredientsImage[i].sprite = recipe.ingredients[i].image;
        }
        for (; i<MAX_INGREDIENT; i++) {
            ingredientsImage[i].sprite = defaultSlotImage;
        }

        


        
    }

    // When Click Type Bookmark
    public void SwitchType(string typeName) {
        OnClickRecipeOff(); // Detail Off
        
        // TypeCasting
        RecipeType switchType = (RecipeType)Enum.Parse(typeof(RecipeType), typeName);        

        if (currentType != switchType) {
            Debug.Log("Setting New RecipeList");
            currentType = switchType;
            // Destroy all RecipeList child component. 
            Transform[] children = recipeListUI.GetComponentsInChildren<Transform>();
            if (children != null) {
                for (int i=1; i<children.Length; i++) {
                    Destroy(children[i].gameObject);
                }
            }

            // Setting New List
            SettingRecipeList(switchType);
        }
    }

    // When Click Recipe
    public void OnClickRecipeOn(RecipeSO recipe) {
        SettingRecipeDetail(recipe);
        // recipeListUI.SetActive(false);

        AudioManager.Inst.PlayOneShot(SoundName.SFX_PageDetail);
        widgetTrigger.Show("Recipe Detail");
        // recipeDetailUI.SetActive(true);
    }

    // When Click Recipe Off
    public void OnClickRecipeOff() {
        // recipeListUI.SetActive(true);
        // recipeDetailUI.SetActive(false);
        // widgetTrigger.Close("Recipe Detail");

        if (TutorialManager.tutoInstance != null 
            && TutorialManager.tutoInstance.GetTutorialProgress() == 6) {
            TutorialManager.tutoInstance.EnterDialogueMode();
        }
    }

    private void SpawnNoteRecipe(RecipeSO recipe) {
        GameObject newRecipe = Instantiate(pfRecipeBtn, recipeListUI.transform);
        NoteRecipe noteRecipe = newRecipe.GetComponent<NoteRecipe>();
        noteRecipe.InitializeRecipe(recipe, this);
    }

    // Button Event
    public void ButtonEvent_NoteBack()
    {
        LevelLoader levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();

        string backScene = PlayerPrefs.GetString("BeforeNote");
        levelLoader.LoadScene(backScene);
    }

    // public void ButtonEvent_RecipeType(string type) 
    // {
    //     recipeTypeText.text = type;
    // }

    // private void SpawnNoteRecipeNone() {
    //    Instantiate(pfRecipeNone, recipeListUI.transform);
    // }

    public void LoadData(GameData data)
    {
        this.recipesCollected = data.recipesCollected;
    }

    public void SaveData(GameData data)
    {
        // throw new NotImplementedException();
    }
}
