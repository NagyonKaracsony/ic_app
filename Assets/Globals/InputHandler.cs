using Assets;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class InputHandler : MonoBehaviour
{
    public static bool pauseMenuState = false;
    public static bool isNestedInPauseMenu = false;
    public static bool isInMainMenu = true;
    public static GameObject previousSelectedObject;
    public static GameObject currentSelectedObject;
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

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = Input.mousePosition;
                    if (Physics.Raycast(CameraController.CurrentCamera.ScreenPointToRay(mousePosition), out RaycastHit raycastHit))
                    {
                        GameObject hitObject = raycastHit.transform.gameObject;
                        int targetLayer = hitObject.layer;
                        switch (targetLayer)
                        {
                            case 6: // Ships
                                DisplayShipUI(hitObject);
                                break;
                            case 7: // Planets
                                DisplayPlanetUI(hitObject);
                                break;
                            case 9: // Stations
                                DisplayStationUI(hitObject);
                                break;
                            case 10: // Sectors
                                if (currentSelectedObject.GetComponent<Battleship>() != null)
                                {
                                    if (currentSelectedObject.GetComponent<Battleship>().ownerID == 0)
                                    {
                                        Vector3 temp = raycastHit.point;
                                        temp.y = 0f;
                                        currentSelectedObject.GetComponent<NavMeshAgent>().SetDestination(temp);
                                    }
                                }
                                break;
                        }
                        previousSelectedObject = currentSelectedObject;
                        currentSelectedObject = raycastHit.transform.gameObject;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.UIPanels.Count != 0) KillLastUIPanel();
                else TogglePauseMenu();
            }
        }
    }
    private void DisplayShipUI(GameObject hitObject)
    {

    }
    private void DisplayPlanetUI(GameObject hitObject)
    {

    }
    private void DisplayStationUI(GameObject hitObject)
    {

    }
    public void TogglePauseMenu()
    {
        ReferenceHolder.Instance.PauseMenu.SetActive(!pauseMenuState);
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