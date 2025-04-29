using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets
{
    public class ReferenceHolder : MonoBehaviour
    {
        public static ReferenceHolder _instance;
        public Camera MainCamera;
        public Camera SectorCamera;
        public Canvas MainCanvas;

        public GameObject StarPrefab;
        public GameObject PauseMenu;
        public GameObject UIPrefab;
        public GameObject CanvasTopSide;

        public GameObject ShipUI;
        public GameObject PlanetUI;
        public GameObject StationUI;

        public GameObject StealthShipModel;
        public GameObject InterceptorShipModel;

        public static GameObject[] Resources = new GameObject[5];

        public Material planetMaterial;
        public Material starMaterial;
        public Material sectorMaterial;
        public Material atmosphereMaterial;
        public Material shipMaterial;
        public ReferenceHolder()
        {

        }
        public static ReferenceHolder Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ReferenceHolder>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new();
                        _instance = singletonObject.AddComponent<ReferenceHolder>();
                        singletonObject.name = typeof(ReferenceHolder).ToString() + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return _instance;
            }
        }
        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            CanvasTopSide = GameObject.Find("TopSide");
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Main camera needs to be re-referenced in each scene
            Instance.MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            if (scene.name == "MainMenu")
            {

            }
            else
            {
                Resources = new GameObject[]
                {
                    CanvasTopSide.gameObject.transform.GetChild(2).gameObject,
                    CanvasTopSide.gameObject.transform.GetChild(3).gameObject,
                    CanvasTopSide.gameObject.transform.GetChild(4).gameObject,
                    CanvasTopSide.gameObject.transform.GetChild(5).gameObject,
                    CanvasTopSide.gameObject.transform.GetChild(6).gameObject,
                };
            }
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}