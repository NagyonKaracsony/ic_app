using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets
{
    public class CameraController : MonoBehaviour
    {
        public Camera MainCamera;
        public Camera SystemCamera;
        private static Camera[] Cameras;
        public static int CameraIndex = 0;
        public static ICameraController[] Controllers;
        public static Camera CurrentCamera;
        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void Start()
        {
            MainCamera = ReferenceHolder.Instance.MainCamera;
            SystemCamera = ReferenceHolder.Instance.SectorCamera;

            Cameras = new Camera[] { MainCamera, SystemCamera };
            Controllers = new ICameraController[]
            {
                MainCamera.GetComponent<MainCameraController>(),
                SystemCamera.GetComponent<SectorCameraController>(),
            };
            CurrentCamera = Cameras[0];
        }
        public void CycleCamera()
        {
            Cameras[CameraIndex].enabled = false;
            CameraIndex = (CameraIndex + 1) % Cameras.Length;
            Cameras[CameraIndex].enabled = true;
            CurrentCamera = Cameras[CameraIndex];
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "MainMenu")
            {
                MainCamera = ReferenceHolder.Instance.MainCamera;
                SystemCamera = ReferenceHolder.Instance.SectorCamera;

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