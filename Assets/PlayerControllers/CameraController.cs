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

        public float positionThreshold = 0.01f; // Distance threshold
        public float rotationThreshold = 0.1f;  // Angle threshold in degrees

        private Vector3 lastCamPosition;
        private Quaternion lastCamRotation;

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

            // Check if camera moved or rotated significantly
            bool positionChanged = Vector3.Distance(CurrentCamera.transform.position, lastCamPosition) > positionThreshold;
            bool rotationChanged = Quaternion.Angle(CurrentCamera.transform.rotation, lastCamRotation) > rotationThreshold;

            if (positionChanged || rotationChanged)
            {
                lastCamPosition = CurrentCamera.transform.position;
                lastCamRotation = CurrentCamera.transform.rotation;
                FaceChildren();
            }
            if (!MainCamera.IsDestroyed() && !SystemCamera.IsDestroyed() && !InputHandler.pauseMenuState) Controllers[CameraIndex].HandleCameraMovement();
        }
        void FaceChildren()
        {
            Quaternion lookRotation = Quaternion.LookRotation(CurrentCamera.transform.forward, CurrentCamera.transform.up);
            foreach (Transform child in ReferenceHolder.Instance.WorldSpaceCanvas.gameObject.transform) child.rotation = lookRotation;
        }
    }
}