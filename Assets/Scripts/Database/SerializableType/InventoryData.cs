using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public int slotId;
    public string slotType;
    public string itemId;
    public int count;
    
    public InventoryData(int _slotId, string _slotType, string _itemId, int _count) {
        slotId = _slotId;
        slotType = _slotType;
        itemId = _itemId;
        count = _count; 
    }

}
