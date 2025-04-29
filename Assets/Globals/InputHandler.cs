using Assets;
using Assets.Globals;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class InputHandler : MonoBehaviour
{
    public static bool pauseMenuState = false;
    public static bool isNestedInPauseMenu = false;
    public static bool isInMainMenu = true;
    public static GameObject previousSelectedObject = null;
    public static GameObject currentSelectedObject = null;
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
                if (Input.GetKeyDown(KeyCode.Z)) GlobalUtility.SaveGame();
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = Input.mousePosition;
                    if (Physics.Raycast(CameraController.CurrentCamera.ScreenPointToRay(mousePosition), out RaycastHit raycastHit))
                    {
                        GameObject hitObject = raycastHit.transform.gameObject;
                        int targetLayer = hitObject.layer;

                        previousSelectedObject = currentSelectedObject;
                        currentSelectedObject = raycastHit.transform.gameObject;

                        if (previousSelectedObject != null)
                        {
                            if (currentSelectedObject.GetHashCode() == previousSelectedObject.GetHashCode()) return;
                        }

                        switch (targetLayer)
                        {
                            case 6: // Ships
                                currentSelectedObject.GetComponent<Battleship>().ShowRange();
                                DisplayShipUI(hitObject);
                                break;
                            case 7: // Planets
                                DisplayPlanetUI(hitObject);
                                break;
                            case 9: // Stations
                                DisplayStationUI(hitObject);
                                break;
                            case 10: // Sectors
                                if (previousSelectedObject != null)
                                {
                                    if (previousSelectedObject.layer == 6)
                                    {
                                        Battleship battleship = previousSelectedObject.GetComponent<Battleship>();
                                        if (battleship.ownerID == 0)
                                        {
                                            Vector3 temp = raycastHit.point;
                                            temp.y = 0f;
                                            battleship.GetComponent<NavMeshAgent>().SetDestination(temp);
                                        }
                                    }
                                }
                                break;
                        }
                        if (previousSelectedObject != null)
                        {
                            if (previousSelectedObject.layer == 6) previousSelectedObject.GetComponent<Battleship>().HideRange();
                        }
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