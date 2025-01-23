using UnityEngine;
namespace Assets
{
    public class CameraController : MonoBehaviour
    {
        [Header("Focus Settings")]
        public Vector3 FocusPoint; // The point around which the camera orbits
        public float DistanceFromFocus = 10f; // The distance between the camera and the focus point
        [Header("Camera Settings")]
        public float OrbitSpeed = 50f; // Speed of orbital rotation
        public float ZoomSpeed = 2f; // Speed of zooming
        public float MinZoomDistance = 5f;
        public float MaxZoomDistance = 20f;

        public float CameraMoveSpeed;
        private float currentYRotation = 0f; // Current Y rotation angle
        void Start()
        {
            if (FocusPoint == null)
            {
                FocusPoint = new Vector3(0, 0, 0);
                return;
            }
            else
            {
                Debug.Log($"Camera focusPoint is set to {FocusPoint}");
            }
        }
        void Update()
        {
            HandleCameraMovement();
            if (Input.GetMouseButtonDown(0)) CastRay();
        }
        void CastRay()
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out RaycastHit raycastHit))
            {
                try
                {
                    Debug.Log(raycastHit.transform.GetComponent<Planet>().resolution);
                }
                catch (System.Exception)
                {
                    Debug.Log(raycastHit.transform.name);
                }
            }
        }
        private void HandleCameraMovement()
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0; // Ignore vertical movement
            right.y = 0;   // Ignore vertical movement

            forward.Normalize();
            right.Normalize();

            if (Input.GetKey(KeyCode.W)) FocusPoint += forward * CameraMoveSpeed / 10;
            if (Input.GetKey(KeyCode.A)) FocusPoint -= right * CameraMoveSpeed / 10;
            if (Input.GetKey(KeyCode.S)) FocusPoint -= forward * CameraMoveSpeed / 10;
            if (Input.GetKey(KeyCode.D)) FocusPoint += right * CameraMoveSpeed / 10;

            transform.position = Vector3.Lerp(transform.position, FocusPoint, 0.1f);

            if (Input.GetKey(KeyCode.E)) currentYRotation += OrbitSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Q)) currentYRotation -= OrbitSpeed * Time.deltaTime;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            DistanceFromFocus -= scroll * ZoomSpeed;
            DistanceFromFocus = Mathf.Clamp(DistanceFromFocus, MinZoomDistance, MaxZoomDistance);

            // Calculate the new camera position
            Quaternion rotation = Quaternion.Euler(20f, currentYRotation, 0f);
            Vector3 offset = rotation * Vector3.back * DistanceFromFocus; // Backward direction relative to rotation
            transform.position = FocusPoint + offset;

            // Look at the focus point
            transform.LookAt(FocusPoint);
        }
    }
}