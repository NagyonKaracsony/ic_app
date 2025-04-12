using UnityEngine;
namespace Assets
{
    public class SectorCameraController : MonoBehaviour, ICameraController
    {
        public Vector3 FocusPoint;
        private float DistanceFromFocus = 10f;
        private float CameraZoomSpeed = 2;
        private float CameraMoveSpeed = 30;
        public void HandleCameraMovement()
        {
            Vector3 forward = transform.up;
            Vector3 right = transform.right;

            if (Input.GetKey(KeyCode.W)) if (FocusPoint.z <= 120) FocusPoint += forward * CameraMoveSpeed * Time.unscaledDeltaTime * DistanceFromFocus / 10;
            if (Input.GetKey(KeyCode.A)) if (FocusPoint.x >= -120) FocusPoint -= right * CameraMoveSpeed * Time.unscaledDeltaTime * DistanceFromFocus / 10;
            if (Input.GetKey(KeyCode.S)) if (FocusPoint.z >= -120) FocusPoint -= forward * CameraMoveSpeed * Time.unscaledDeltaTime * DistanceFromFocus / 10;
            if (Input.GetKey(KeyCode.D)) if (FocusPoint.x <= 120) FocusPoint += right * CameraMoveSpeed * Time.unscaledDeltaTime * DistanceFromFocus / 10;
            transform.position = Vector3.Lerp(transform.position, FocusPoint, 0.1f);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            DistanceFromFocus -= scroll * (CameraZoomSpeed * DistanceFromFocus);
            DistanceFromFocus = Mathf.Clamp(DistanceFromFocus, 2f, 100);
            Vector3 offset = Quaternion.identity * Vector3.up * DistanceFromFocus;
            transform.position = FocusPoint + offset;
            transform.LookAt(FocusPoint);
        }
    }
}