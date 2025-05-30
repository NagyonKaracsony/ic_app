using Assets;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public static bool isStartingNewGame = false;
    public Animator transition;
    public void StartNewGame()
    {
        isStartingNewGame = true;
        StartCoroutine(LoadSceneAsynchronously(1));
    }
    public void BackToMainMenu()
    {
        StartCoroutine(LoadSceneAsynchronously(0));
    }
    public void BackToDesktop()
    {
        StartCoroutine(ExitGame());
    }
    IEnumerator ExitGame()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
    IEnumerator LoadSceneAsynchronously(int sceneIndex)
    {
        GameTimeHandler.IsLoading = true; // Set loading state to true
        if (sceneIndex == 0)
        {
            FindAnyObjectByType<InputHandler>().TogglePauseMenu();
            FindAnyObjectByType<GameTimeHandler>().SetInGameTime(1);
            InputHandler.isInMainMenu = true;
        }
        else InputHandler.isInMainMenu = false;

        transition.SetTrigger("Start"); // Play scene change animation
        yield return new WaitForSeconds(1f); // Wait for the animation to finish
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex); // Load the scene asynchronously
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }
}