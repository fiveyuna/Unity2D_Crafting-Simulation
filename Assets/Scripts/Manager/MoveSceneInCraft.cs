using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSceneInCraft : MonoBehaviour
{
    private InventoryManager inventoryManager;
    [SerializeField] CraftingManager craftingManager;

    public static MoveSceneInCraft moveSceneInCraft;

    private void Awake()
    {
        inventoryManager = InventoryManager.inventoryInstance;
        moveSceneInCraft = this;
    }

    public void LoadMainFromCraft() {
        Debug.Log("Move to main after Data set");
        AddSlotItemData();
        SceneManager.LoadScene("Main");
    }

    public void LoadNoteFromCraft() {
        PlayerPrefs.SetString("BeforeNote", "Craft");

        AddSlotItemData();
        Debug.Log("Move to note after Data set");
        
        SceneManager.LoadScene("Note");
    }

    public void AddSlotItemData() {
        // add ouput slot item
        if (craftingManager.outputSlot.childCount > 0) {
            inventoryManager.AddItem(craftingManager.createdItem, craftingManager.ingredientsCount);
            Debug.Log("Test. AddSlotItemData() output item : " + craftingManager.createdItem.name + ", count : " + craftingManager.ingredientsCount);

        }

        // add input slot item
        foreach (CraftSlot slot in craftingManager.craftSlots) {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) {
                inventoryManager.AddItem(itemInSlot.item, craftingManager.ingredientsCount);
                Debug.Log("Test. AddSlotItemData() item : " + itemInSlot.item.name + ", count : " + craftingManager.ingredientsCount);
            }
        }

        inventoryManager.InventoryItemListToDataList();
    }
}
