using UnityEngine;
namespace Assets
{
    public class SectorCameraController : MonoBehaviour, ICameraController
    {
        public Vector3 FocusPoint;
        public float DistanceFromFocus;
        public float CameraZoomSpeed;
        public float CameraMoveSpeed;
        public void HandleCameraMovement()
        {
            Vector3 forward = transform.up;
            Vector3 right = transform.right;

            if (Input.GetKey(KeyCode.W))
            {
                if (FocusPoint.z <= 75) FocusPoint += forward * CameraMoveSpeed / 10 * DistanceFromFocus / 10;
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (FocusPoint.x >= -75) FocusPoint -= right * CameraMoveSpeed / 10 * DistanceFromFocus / 10;
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (FocusPoint.z >= -75) FocusPoint -= forward * CameraMoveSpeed / 10 * DistanceFromFocus / 10;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (FocusPoint.x <= 75) FocusPoint += right * CameraMoveSpeed / 10 * DistanceFromFocus / 10;
            }
            transform.position = Vector3.Lerp(transform.position, FocusPoint, 0.1f);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            DistanceFromFocus -= scroll * (CameraZoomSpeed * DistanceFromFocus);
            DistanceFromFocus = Mathf.Clamp(DistanceFromFocus, 1f, 50f);
            Vector3 offset = Quaternion.identity * Vector3.up * DistanceFromFocus;
            transform.position = FocusPoint + offset;
            transform.LookAt(FocusPoint);
        }
    }
}