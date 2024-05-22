using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Start,
    Dialogue,
    Main,
    Craft,
    Note,
    Study,
    Adv_select,
    Adv_main
}

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transition;

    [SerializeField]
    private float transitionTime = 1f;

    public void LoadScene(SceneName sceneName)
    {
        LoadScene(sceneName.ToString());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadSceneAsync(levelName);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load scene
        SceneManager.LoadSceneAsync(levelIndex);
    }

    // public void LoadSceneWithTime_03(string sceneName)
    // {
    //     transitionTime = 0.3f;
    //     StartCoroutine(LoadLevel(sceneName));
    // }

    // public void LoadSceneWithoutTransition(string sceneName)
    // {
    //     SceneManager.LoadSceneAsync(sceneName);
    // }
}