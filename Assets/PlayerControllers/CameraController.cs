using UnityEngine;
namespace Assets
{
    public class CameraController : MonoBehaviour
    {
        public Camera MainCamera;
        public Camera SystemCamera;
        private int CameraIndex = 0;
        private Camera[] Cameras;
        private ICameraController[] Controllers;
        void Start()
        {
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
        private void Update()
        {
            Controllers[CameraIndex].HandleCameraMovement();
        }
    }
}