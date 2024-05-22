using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Localization;
// using UnityEngine.Localization.Settings;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    private TextAsset inkJSON_mouse;
    private TextAsset inkJSON_monster;
    [SerializeField] private TextAsset mouse_en;
    [SerializeField] private TextAsset monster_en;
    [SerializeField] private TextAsset mouse_kr;
    [SerializeField] private TextAsset monster_kr;

    private static DialogueTrigger instance;

    private void Awake()
    {
        if (instance != null) {
            Debug.LogWarning("Found more than one Dialogue Trigger in the scene");
        }
        
        instance = this;

        // dialogueVariables = new DialogueVariables(loadGlobalsJSON);

        int localIndex = PlayerPrefs.HasKey("Locale") ? PlayerPrefs.GetInt("Locale") : 1;
        // Debug.Log("[Test] local : " + localIndex);
        if (localIndex == 0) { // en
            inkJSON_mouse = mouse_en;
            inkJSON_monster = monster_en;
        } else {
            inkJSON_mouse = mouse_kr;
            inkJSON_monster = monster_kr;
        }
        
        // Locale currentLocale = LocalizationSettings.SelectedLocale;
        // if (currentLocale == LocalizationSettings.AvailableLocales.Locales[0]) { // en
        //     inkJSON_mouse = mouse_en;
        //     inkJSON_monster = monster_en;
        // } else if (currentLocale == LocalizationSettings.AvailableLocales.Locales[1]) { // kr
        //     inkJSON_mouse = mouse_kr;
        //     inkJSON_monster = monster_kr;
        // } else {
        //     Debug.Log("locale : error");
        // }
    }

    public static DialogueTrigger GetInstance()
    {
        return instance;
    }

    // Dialogue Triggers (JSON)
    public void TriggerDialogue(string jsonName, string advName, int depth)
    {
        bool isMouse = AdventureManager.GetInstance().storyProgress.Contains("mouse");
        bool isStranger = AdventureManager.GetInstance().storyProgress.Contains("stranger");

        string knotName = "adv_" + advName + "_" + depth;

        if (jsonName == "mouse") {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON_mouse, knotName);
        } else if (jsonName == "monster") {
            if (isMouse) {
                if (depth == 0 && advName == "mine" ) {
                    knotName = isStranger ? "off_mine_0_true" : "off_mine_0_false";
                } else if (depth == 30 && advName == "sea") {
                    
                } 
                else if (depth == 10 || depth == 20) {
                    knotName = "choice_common_true";
                } 
                else {
                    return;
                }
            }
            
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON_monster, knotName);   
        }
        
    }

}
