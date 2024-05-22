using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnlockSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private AdventureManager adventureManager;
    [SerializeField] private AdvSelectManager advSelectManager;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        // Debug.Log("OnDrop child count: " + transform.childCount +", unlock: " + adventureManager.unlockItem);

        if (transform.childCount == 0) {
            if (adventureManager != null && inventoryItem.item == adventureManager.unlockItem) {
                AudioManager.Inst.PlayOneShot(SoundName.SFX_Unlock);
                SetInventoryItem();
                
                adventureManager.UnlockTheSlot();

                // Debug.Log("[TEST] UnlockSlot - inven : " + inventoryItem.item.ToString() + ", adv : " + adventureManager.unlockItem.ToString());
            } else if (advSelectManager != null && inventoryItem.item == advSelectManager.unlockItem) {
                AudioManager.Inst.PlayOneShot(SoundName.SFX_Unlock);
                SetInventoryItem();
                
                advSelectManager.UnlockTheSlot();
            } else {
                Debug.LogError("This is wrong unlockSlot.");
            }
        }

        void SetInventoryItem() {
            inventoryItem.parentAfterDrag = transform;
            inventoryItem.isChild = false;
            Destroy(inventoryItem.gameObject);
        }
    }

    
}
