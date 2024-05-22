using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteRecipe : MonoBehaviour
{
    [Header("UI")]
    public Image image; // @꼮 public??
    public Button button;
    [Header("GAME")]
    public RecipeSO recipe;

    public void InitializeRecipe(RecipeSO recipe, NoteManager noteMng) {
        this.recipe = recipe;
        image.sprite = recipe.outputItem.image;

        if (this.recipe.id != -1) { // collected recipe
            button.onClick.AddListener(() => noteMng.OnClickRecipeOn(recipe));
        }
        //             

    }
}
