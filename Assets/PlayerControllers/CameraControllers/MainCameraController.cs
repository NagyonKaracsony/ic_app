using UnityEngine;

namespace Assets
{
    public class MainCameraController : MonoBehaviour, ICameraController
    {
        public Vector3 FocusPoint;
        private float DistanceFromFocus = 10f;
        private static float OrbitSpeed = 30f;
        private float CameraZoomSpeed = 2f;
        private float CameraMoveSpeed = 30f;
        private float currentYRotation = 0f;
        public void HandleCameraMovement()
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            if (Input.GetKey(KeyCode.W)) FocusPoint += forward * CameraMoveSpeed * Time.unscaledDeltaTime * (DistanceFromFocus / 10);
            if (Input.GetKey(KeyCode.A)) FocusPoint -= right * CameraMoveSpeed * Time.unscaledDeltaTime * (DistanceFromFocus / 10);
            if (Input.GetKey(KeyCode.S)) FocusPoint -= forward * CameraMoveSpeed * Time.unscaledDeltaTime * (DistanceFromFocus / 10);
            if (Input.GetKey(KeyCode.D)) FocusPoint += right * CameraMoveSpeed * Time.unscaledDeltaTime * (DistanceFromFocus / 10);

            transform.position = Vector3.Lerp(transform.position, FocusPoint, 0.1f);

            if (Input.GetKey(KeyCode.E)) currentYRotation -= OrbitSpeed * Time.unscaledDeltaTime;
            if (Input.GetKey(KeyCode.Q)) currentYRotation += OrbitSpeed * Time.unscaledDeltaTime;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            DistanceFromFocus -= scroll * (CameraZoomSpeed * DistanceFromFocus);
            DistanceFromFocus = Mathf.Clamp(DistanceFromFocus, 2f, 100);

            Quaternion rotation = Quaternion.Euler(40f, currentYRotation, 0f);
            Vector3 offset = rotation * Vector3.back * DistanceFromFocus;
            transform.position = FocusPoint + offset;

            transform.LookAt(FocusPoint);
        }
    }
}