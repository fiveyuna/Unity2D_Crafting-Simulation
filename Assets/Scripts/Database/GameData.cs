using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int energy;
    public int day;
    public List<string> recipesCollected; // key : "type-id" : ex. Basic-0

    public int[] advDepth; // 숲, 바다, 광산, 오두막
    public bool isMineUnlocked; // 광산 지역 해금 
    public bool[] isCraftUnlocked; // 연구도구 해금
    public List<InventoryData> inventoryList;

    public List<string> storyProgress;
    //  - mouse : true -> 쥐를 따돌림
    //  - honey : true -> 꿀병 획득
    //  - poison : true -> 독약 획득
    //  - stranger : true -> 수상한 자와 만남.
    //  - complete : true -> 연구도구 완성
    //  - intro : true -> 인트로 단계 (New Game)
    //  - tutorial

    // Initialize Field
    public GameData() {
        // hp = 3;
        energy = 30;
        day = 1;
        recipesCollected = new List<string>() {"Main1-9", "Main1-10"};
        
        ClearAdvDepth();
        
        isMineUnlocked = false;
        isCraftUnlocked = new bool[]{false, false};

        storyProgress = new List<string> {"intro", "tutorial"};

        inventoryList = new List<InventoryData>();
    }

    public void ClearAdvDepth()
    {
        advDepth = new int[] {0, 0, 0, 0}; 
    }
}
