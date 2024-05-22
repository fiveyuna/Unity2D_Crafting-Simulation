using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecipeNameStr
{
    public static Dictionary<RecipeType,string> recipeName_kr = new Dictionary<RecipeType,string>()
    {
        { RecipeType.Basic, "기본 조합법" },
        { RecipeType.Main1, "연구도구 조합법" },
        { RecipeType.Depth, "탐험의 열쇠 조합법" },
        { RecipeType.Event, "선물 조합법" },
    };

    public static Dictionary<RecipeType,string> recipeName_en = new Dictionary<RecipeType,string>()
    {
        { RecipeType.Basic, "Basic Recipe" },
        { RecipeType.Main1, "Research Tool Recipe" },
        { RecipeType.Depth, "Adventure Key Recipe" },
        { RecipeType.Event, "Gift Recipe" },
    };

    // public static string GetKRName(RecipeType type)
    // {
    //     return recipeName_kr[type];
    // }

    // public static string GetENName(RecipeType type)
    // {
    //     return recipeName_en[type];
    // }
}
