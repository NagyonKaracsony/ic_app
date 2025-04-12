using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public void StartNewGame()
    {
        StartCoroutine(LoadAsyncronously(1));
    }
    IEnumerator LoadAsyncronously(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.LogWarning($"Loading progress: {operation.progress}%");
            yield return null;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}