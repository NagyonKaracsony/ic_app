using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets
{
    public class CameraController : MonoBehaviour
    {
        public Camera MainCamera;
        public Camera SystemCamera;
        private int CameraIndex = 0;
        private Camera[] Cameras;
        private ICameraController[] Controllers;
        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void Start()
        {
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            SystemCamera = GameObject.Find("SectorCamera").GetComponent<Camera>();

            Cameras = new Camera[] { MainCamera, SystemCamera };
            Controllers = new ICameraController[]
            {
                MainCamera.GetComponent<MainCameraController>(),
                SystemCamera.GetComponent<SectorCameraController>(),
            };
        }
        public void CycleCamera()
        {
            Cameras[CameraIndex].enabled = false;
            CameraIndex = (CameraIndex + 1) % Cameras.Length;
            Cameras[CameraIndex].enabled = true;
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "MainMenu")
            {
                MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
                SystemCamera = GameObject.Find("SectorCamera").GetComponent<Camera>();

                Cameras = new Camera[] { MainCamera, SystemCamera };
                Controllers = new ICameraController[]
                {
                    MainCamera.GetComponent<MainCameraController>(),
                    SystemCamera.GetComponent<SectorCameraController>(),
                };
            }
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void Update()
        {
            if (!MainCamera.IsDestroyed() && !SystemCamera.IsDestroyed() && !InputHandler.pauseMenuState) Controllers[CameraIndex].HandleCameraMovement();
        }
    }
}