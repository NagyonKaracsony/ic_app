using System.Numerics;
namespace Assets
{
    interface ICameraController
    {
        public static Vector3 FocusPoint = new(0, 0, 0);
        public static float DistanceFromFocus = 25f;
        public static float CameraZoomSpeed = 2f;
        public static float CameraMoveSpeed = 2f;
        void HandleCameraMovement();
    }
}