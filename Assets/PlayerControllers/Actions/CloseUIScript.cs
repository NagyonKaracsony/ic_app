using Assets;
using UnityEngine;
public class CloseUI : MonoBehaviour
{
    public void ClosePanel()
    {
        Destroy(gameObject.transform.parent.gameObject); // Destroy the UI panel
    }
    public void TogglePanel()
    {
        gameObject.transform.parent.gameObject.SetActive(false); // Deactivate the UI panel
    }
    public void TogglePauseMenuPanel()
    {
        gameObject.transform.parent.gameObject.SetActive(false); // Deactivate the UI panel
        GameManager.Instance.gameObject.GetComponent<GameTimeHandler>().SetLastGameTime(); // Pause the game
        InputHandler.pauseMenuState = !InputHandler.pauseMenuState;
    }
}