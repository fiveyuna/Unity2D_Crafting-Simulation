using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class EditorStartInit
{
    // static EditorStartInit()
    // {
    //     var pathOfFirstScene = EditorBuildSettings.scenes[1].path; // 씬 번호를 넣어주자.
    //     var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
    //     EditorSceneManager.playModeStartScene = sceneAsset;
    //     Debug.Log(pathOfFirstScene + " 씬이 에디터 플레이 모드 시작 씬으로 지정됨");
    // }


    [MenuItem("MyEditor/시작 씬부터 시작")]
    public static void SetupFromStartScene()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
        UnityEditor.EditorApplication.isPlaying = true;
    }

    [MenuItem("MyEditor/현재 씬부터 시작")]
    public static void StartFromThisScene()
    {
        EditorSceneManager.playModeStartScene = null;
        UnityEditor.EditorApplication.isPlaying = true;
    }
}