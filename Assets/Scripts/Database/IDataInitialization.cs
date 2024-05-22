using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// use in dont destroy datas.
public interface IDataInitialization
{
    void LoadData(GameData data);
    void SaveData(GameData data);
}
