using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
// using UnityEngine.Localization;
// using UnityEngine.Localization.Settings;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI nameText;

    [HideInInspector] public Transform parentAfterDrag;
    [ReadOnly(true)] public int count = 1; 
    [HideInInspector] public ItemSO item;

    [HideInInspector] public bool isChild = false;

    private void Awake() {
        // image = GetComponent<Image>();
        // countText = GetComponentInChildren<TextMeshProUGUI>();

        RefreshCount();
    }

    public void InitializeItem(ItemSO item) {
        this.item = item;
        image.sprite = item.image;
        SetItemName();

        // Locale currentLocale = LocalizationSettings.SelectedLocale;
        // string language = "kr";
        // if (currentLocale == LocalizationSettings.AvailableLocales.Locales[0]) { // en
        //     language = "en";
        // }
        // string itemStr = language == "kr" ? item.itemName : item.itemName_en;
        // nameText.text = itemStr;
    }

    public void RefreshCount() {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);

        if (count < 1) {
            Destroy(this.gameObject);
        }
    }

    public void SetItemName()
    {
        int localIndex = PlayerPrefs.HasKey("Locale") ? PlayerPrefs.GetInt("Locale") : 1;
        // Debug.Log("[Test] local : " + localIndex);

        string itemStr;
        if (localIndex == 0) { // en
            itemStr = item.itemName_en;
        } else {
            itemStr = item.itemName;
        }

        // Locale currentLocale = LocalizationSettings.SelectedLocale;
        // string language = "kr";
        // if (currentLocale == LocalizationSettings.AvailableLocales.Locales[0]) { // en
        //     language = "en";
        // }
        
        nameText.text = itemStr;
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

        if (count > 1) {
            // new item initialize 
            count--;
            GameObject newItem = Instantiate(this.gameObject, parentAfterDrag);
            InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
            inventoryItem.gameObject.name = inventoryItem.gameObject.name.Replace("(Clone)","").Trim();

            inventoryItem.InitializeItem(item);
            inventoryItem.image.raycastTarget = true;
            
            // this item setting
            this.count = 1;
            this.RefreshCount();

            isChild = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var canvas = image.canvas;
        if (canvas.worldCamera != null) {
            Vector3 position = eventData.position;
            position.z = canvas.planeDistance;
            image.transform.position = canvas.worldCamera.ScreenToWorldPoint(position);
        } else {
            transform.position = Input.mousePosition;
        }       
    }

    public void OnEndDrag(PointerEventData eventData)
    {        
        // Debug.Log("OnEndDrag");

        // If Not OnDrop -> Destroy this object
        if (isChild) { // Not OnDrop. Combine with parent
            InventoryItem parentItem = parentAfterDrag.GetComponentInChildren<InventoryItem>();
            parentItem.count++;
            parentItem.RefreshCount();
            Destroy(this.gameObject);
        } else { // OnDrop
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
        }


    }
}
