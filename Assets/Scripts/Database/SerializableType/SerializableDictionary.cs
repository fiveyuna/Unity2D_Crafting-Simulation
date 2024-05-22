using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    
    // save dic to list
    public void OnBeforeSerialize() 
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this) {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dic from list
    public void OnAfterDeserialize() 
    {
        this.Clear(); // Dic Clear

        if (keys.Count != values.Count) {
            Debug.LogError("[SerializableDictionary] does not match with key and value. keys count : " + keys.Count + " and values count : " + values.Count + "." );
        }

        for (int i=0; i<keys.Count; i++) {
            this.Add(keys[i], values[i]);
        }
    }
}
