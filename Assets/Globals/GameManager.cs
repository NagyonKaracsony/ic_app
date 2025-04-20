using Assets;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static Civilization playerCivilization;
    public static List<GameObject> UIPanels = new();
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }
    public void SaveGame()
    {

    }
    public void LoadGame()
    {

    }
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
        UIPanels.Add(GameObject.Find("EventUI"));
        playerCivilization = new Civilization();
    }
}