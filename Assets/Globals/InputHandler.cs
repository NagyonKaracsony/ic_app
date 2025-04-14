using Assets;
using System.Linq;
using UnityEngine;
public class InputHandler : MonoBehaviour
{
    public static bool pauseMenuState = false;
    public static bool isNestedInPauseMenu = false;
    public static bool isInMainMenu = true;
    void Update()
    {
        if (!isInMainMenu)
        {
            if (!pauseMenuState)
            {
                // not the best way to do this, but it works for now
                if (Input.GetKeyDown(KeyCode.Alpha0)) transform.gameObject.GetComponent<GameTimeHandler>().SetInGameTime(0);
                else if (Input.GetKeyDown(KeyCode.Alpha1)) transform.gameObject.GetComponent<GameTimeHandler>().SetInGameTime(1);
                else if (Input.GetKeyDown(KeyCode.Alpha2)) transform.gameObject.GetComponent<GameTimeHandler>().SetInGameTime(2);
                else if (Input.GetKeyDown(KeyCode.Alpha3)) transform.gameObject.GetComponent<GameTimeHandler>().SetInGameTime(3);
                else if (Input.GetKeyDown(KeyCode.Alpha4)) transform.gameObject.GetComponent<GameTimeHandler>().SetInGameTime(4);
                else if (Input.GetKeyDown(KeyCode.Alpha5)) transform.gameObject.GetComponent<GameTimeHandler>().SetInGameTime(5);

                // Handling camera cycling
                if (Input.GetKeyDown(KeyCode.M)) transform.gameObject.GetComponent<CameraController>().CycleCamera();
            }

            // Handling pause menu toggling
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.UIPanels.Count != 0) KillLastUIPanel();
                else TogglePauseMenu();
            }
        }
    }
    public void TogglePauseMenu()
    {
        FindObjectOfType<RefrenceHolder>().PauseMenu.SetActive(!pauseMenuState);
        if (pauseMenuState) transform.gameObject.GetComponent<GameTimeHandler>().SetLastGameTime();
        else transform.gameObject.GetComponent<GameTimeHandler>().SetInGameTime(0);
        pauseMenuState = !pauseMenuState;
    }
    public void KillLastUIPanel()
    {
        Destroy(GameManager.UIPanels.Last());
        GameManager.UIPanels.RemoveAt(GameManager.UIPanels.Count - 1);
    }
    public void OnClickKillPanel()
    {
        Destroy(gameObject.transform.parent.gameObject); // Destroy the UI panel
    }
    public void OnClickTogglePanel()
    {
        gameObject.transform.parent.gameObject.SetActive(false); // Deactivate the UI panel
    }
    public void OpenSavePanel()
    {
        isNestedInPauseMenu = true;
    }
    public void OpenLoadPanel()
    {
        isNestedInPauseMenu = true;
    }
    public void OpenSettingsPanel()
    {
        isNestedInPauseMenu = true;
    }
}