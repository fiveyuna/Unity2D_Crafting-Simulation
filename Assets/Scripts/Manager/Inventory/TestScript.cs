using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public ItemSO[] itemsToSpawn;

    private void Awake() {
        inventoryManager  = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    public void SpawnItem(int id) {
        bool result = inventoryManager.AddItem(itemsToSpawn[id], 1);
        if (result) {
            // Debug.Log("들감");
        } else {
            // Debug.Log("안 들감");
        }
    }

    public void MoveInForest() {
        int randId = Random.Range(0, itemsToSpawn.Length);
        bool result = inventoryManager.AddItem(itemsToSpawn[randId], 1);
        if (result) {
            Debug.Log("들감 : " + itemsToSpawn[randId].ToString());
        } else {
            Debug.Log("안 들감");
        }
    }
}
