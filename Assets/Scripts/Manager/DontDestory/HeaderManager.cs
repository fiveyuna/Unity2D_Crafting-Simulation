using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeaderManager : MonoBehaviour, IDataInitialization
{
    [Header("Header UI")]
    // [SerializeField] private Image[] hpImages;
    [SerializeField] private Slider energyBar;
    private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI calendarText;

    [Header("Header data")]
    // [HideInInspector] public int hp;
    [HideInInspector] public int energy;
    [HideInInspector] public int day = 1;

    [Header("Const Value")]
    // private const int MAX_HP = 3;
    private const int MAX_ENERGY = 30;

    [Header("Game")]
    private static HeaderManager headerInstance; 

    // [Header("Resource")]
    // [SerializeField] private Sprite[] hpSprites; // 0:full, 1:empty

    private void Awake()
    {
        headerInstance = this;

        energyText = energyBar.GetComponentInChildren<TextMeshProUGUI>();
        
        SetHeaderDataDefault();
        // SetCalendarText();
    }

    public static HeaderManager GetInstance()
    {
        return headerInstance;
    }

    // Control from Main manager
    public void MoveToNextDay()
    {

        day++;
        SetCalendarText();
    }

// [ Util ]
    public void SetHeaderDataDefault() {
        // hp = MAX_HP;
        energy = MAX_ENERGY;

        // SetHpUI(); 
        SetEnergyUI();
    }

    public void SetCalendarText() {
        calendarText.text = "" + day;
        // Debug.Log("[Test] SetCalendarText. day : " + day);
    }

    // public void SetHeaderUI() {
    //     // SetHpUI(); 
        
    // }

    /*
    private void SetHpUI() {
        int[] hpSpriteArray = new int[3];
        switch (hp) {
            case 0:
                hpSpriteArray = new int[3] {1, 1, 1};
                break;
            case 1:
                hpSpriteArray = new int[3] {0, 1, 1};
                break;
            case 2:
                hpSpriteArray = new int[3] {0, 0, 1};
                break;
            case 3:
                hpSpriteArray = new int[3] {0, 0, 0};
                break;
            default:
                Debug.LogWarning("[HeaderManager] HP data have wrong value.");
                break;
        }    
        ManageHpImages(hpSpriteArray);
    }

    private void ManageHpImages(int[] hpArray) {
        int i = 0;
        foreach (int spriteIndex in hpArray) {
            hpImages[i++].sprite = hpSprites[spriteIndex];
        }
    }
    */

    public void SetEnergyUI() {
        energyBar.value = energy;
        energyText.text = energy.ToString();
    }

    public void LoadData(GameData data)
    {
        this.energy = data.energy;
        this.day = data.day;

        SetEnergyUI();
        SetCalendarText();
    }

    public void SaveData(GameData data)
    {
        data.energy = this.energy;
        data.day = this.day;
    } 
}
