using UnityEngine;
namespace Assets
{
    public class GalaxyCameraController : MonoBehaviour, ICameraController
    {
        public Vector3 FocusPoint;
        public float DistanceFromFocus;
        public float OrbitSpeed = 30f;
        public float CameraZoomSpeed;
        public float CameraMoveSpeed;
        public void HandleCameraMovement()
        {
            // throw new System.NotImplementedException();
        }
    }
}