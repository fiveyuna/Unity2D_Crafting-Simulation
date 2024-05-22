using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftUnlockSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private CraftingManager craftingManager;
    [SerializeField] private ItemSO unlockItem; 
    
    [Tooltip("0 또는 1의 값")]
    [SerializeField] private int partNumber; // 0 or 1

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        // Debug.Log("OnDrop child count: " + transform.childCount +", unlock: " + adventureManager.unlockItem);

        if (transform.childCount == 1 && inventoryItem.item == unlockItem) {
            AudioManager.Inst.PlayOneShot(SoundName.SFX_Unlock);
            inventoryItem.parentAfterDrag = transform;
            inventoryItem.isChild = false;
            Destroy(inventoryItem.gameObject);
            
            craftingManager.UnlockTheSlot(partNumber);
        } 
    }
}
