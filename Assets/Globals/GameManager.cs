using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
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
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this) Destroy(gameObject);

        GameObject ship = new GameObject("Ship");

        Ship myShip = ship.AddComponent<Ship>();

        myShip.SetDestination(new Vector3(50, 0, 50));

        myShip.SetDestination(new Vector3(100, 0, 100));

        myShip.RemoveQueuedDestination(new Vector3(100, 0, 100));

        myShip.SetDestination(new Vector3(300, 0, 400));
    }
}