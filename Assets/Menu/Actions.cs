using UnityEngine;
using UnityEngine.SceneManagement;
public class Actions : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
}