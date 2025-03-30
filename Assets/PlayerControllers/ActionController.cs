using Assets;
using UnityEngine;
public class ActionController : MonoBehaviour
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Cameras[CameraIndex].enabled = false;
            CameraIndex = (CameraIndex + 1) % Cameras.Length;
            Cameras[CameraIndex].enabled = true;
        }
        Controllers[CameraIndex].HandleCameraMovement();
    }
}