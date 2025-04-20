using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingsHandler : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}