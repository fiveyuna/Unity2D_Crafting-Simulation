using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AdvSelectManager : MonoBehaviour, IDataPersistence
{
    [Header("UI")]
    [SerializeField] private Button mineButton;
    [SerializeField] private GameObject mineUnlockSlot;
    // [SerializeField] private GameObject infoPanel;

    [Header("Game")]
    public ItemSO unlockItem;

    private bool isMineUnlocked;
    
    // Start is called before the first frame update
    private void Start()
    {
        // infoPanel.SetActive(false);
        SetAdvSelectUI();
    }

// System
    public void UnlockTheSlot() {
        isMineUnlocked = true;
        SetAdvSelectUI();
    }

    public void SelectAdventure(int advIndex) {
        PlayerPrefs.SetInt("AdventureIndex", advIndex);
    }

// UI
    private void SetAdvSelectUI() {
        mineButton.interactable = isMineUnlocked;
        mineUnlockSlot.SetActive(!isMineUnlocked);
    }

    public void LoadData(GameData data)
    {
        this.isMineUnlocked = data.isMineUnlocked;
    }

    public void SaveData(GameData data)
    {
        data.isMineUnlocked = this.isMineUnlocked;
    }
}
