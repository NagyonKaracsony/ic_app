using UnityEngine;

namespace Assets
{
    public class MainCameraController : MonoBehaviour, ICameraController
    {
        public Vector3 FocusPoint;
        public float DistanceFromFocus;
        public static float OrbitSpeed = 30f;
        public float CameraZoomSpeed;
        public float CameraMoveSpeed;
        public float currentYRotation = 0f;
        public void HandleCameraMovement()
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            if (Input.GetKey(KeyCode.W)) FocusPoint += forward * CameraMoveSpeed / 10 * DistanceFromFocus / 10;
            if (Input.GetKey(KeyCode.A)) FocusPoint -= right * CameraMoveSpeed / 10 * DistanceFromFocus / 10;
            if (Input.GetKey(KeyCode.S)) FocusPoint -= forward * CameraMoveSpeed / 10 * DistanceFromFocus / 10;
            if (Input.GetKey(KeyCode.D)) FocusPoint += right * CameraMoveSpeed / 10 * DistanceFromFocus / 10;

            transform.position = Vector3.Lerp(transform.position, FocusPoint, 0.1f);

            if (Input.GetKey(KeyCode.E)) currentYRotation -= OrbitSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Q)) currentYRotation += OrbitSpeed * Time.deltaTime;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            DistanceFromFocus -= scroll * (CameraZoomSpeed * DistanceFromFocus);
            DistanceFromFocus = Mathf.Clamp(DistanceFromFocus, 1f, 50f);

            Quaternion rotation = Quaternion.Euler(40f, currentYRotation, 0f);
            Vector3 offset = rotation * Vector3.back * DistanceFromFocus;
            transform.position = FocusPoint + offset;

            transform.LookAt(FocusPoint);
        }
    }
}