using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrefabDialogue : MonoBehaviour
{
    public TextMeshProUGUI lineText;
    public GameObject nameFrame;
    public TextMeshProUGUI nameText;
    public GameObject choiceParent;
    public GameObject[] choices;
    public TextMeshProUGUI[] choicesText;
    public Image image;

    private void Awake() {
        nameFrame.SetActive(false);
        image.gameObject.SetActive(false);
        choiceParent.SetActive(false);
    }

    public void Button_MakeChoice(int index)
    {
        DialogueManager_Diary.GetInstance().MakeChoice(index);
    }
}
