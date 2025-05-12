using Assets;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static List<GameObject> UIPanels = new();
    public static List<GameObject> Planets = new();
    public static List<GameObject> Sectors = new();
    public static List<Civilization> Civilizations = new();
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
        Civilizations.Add(new Civilization("Player"));
        Civilizations.Add(new Civilization("Enemy"));
    }
}