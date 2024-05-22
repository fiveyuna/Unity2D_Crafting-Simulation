
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleManager : MonoBehaviour
{
    bool isChanging;

    [SerializeField] private Button enButton;
    [SerializeField] private Button krButton;

    // private void Awake() {
    //     StartCoroutine(ChangeRoutine(LoadLocale()));
    // }

    IEnumerator Start()
    {
        // Debug.Log("[TEST] Localization 초기화 시작");
        // LocalizationSettings.InitializationOperation을 기다림
        yield return LocalizationSettings.InitializationOperation;

        // 언어 초기화 작업 완료 후에 다음 작업 수행
        // Debug.Log("[TEST] Localization 초기화 완료!");
        int localIndex = LoadLocale();
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localIndex];
        SettingButtons(localIndex);
        // 이후 언어 설정을 변경하거나 텍스트를 로드하거나 다른 작업 수행 가능
    }

    private void SettingButtons(int index)
    {
        if (index == 0) {
            enButton.interactable = false;
            krButton.interactable = true;
        } else {
            enButton.interactable = true;
            krButton.interactable = false;
        }
    }

    public void ChangeLocale(int index) // 0:EN, 1:KO
    {
        // Debug.Log("[TEST] ChangeLocale clicked");
        if (isChanging)
            return;
        
        StartCoroutine(ChangeRoutine(index));
        PlayerPrefs.SetInt("Locale", index);

        SettingButtons(index);

        if (TutorialManager.tutoInstance != null) {
            TutorialManager.tutoInstance.SetTutorialLanguage();  
        }
          
    }

    IEnumerator ChangeRoutine(int index)
    {
        isChanging = true;
        // Debug.Log("[TEST] Changing : true ");

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;
        // Debug.Log("[TEST] Changing : false ");
    }

    private int LoadLocale()
    {
        return PlayerPrefs.HasKey("Locale") ? PlayerPrefs.GetInt("Locale") : 1;
    }
}
