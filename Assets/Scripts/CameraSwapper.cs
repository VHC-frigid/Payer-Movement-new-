using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwapper : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera, thirdPersonCamera;

    public enum CameraMode
    {
        FirstPeson,
        ThirdPerson
    }

    [SerializeField] private CameraMode currentCameraMode;

    // Start is called before the first frame update
    void Start()
    {       

        firstPersonCamera = FindAnyObjectByType<FirstPersonCamera>().GetComponent<Camera>();
        thirdPersonCamera = FindAnyObjectByType<ThirdPersonCamera>().GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCamera();
        }
    }

    private void ToggleCamera()
    {
        if(currentCameraMode == CameraMode.FirstPeson)
        {
            currentCameraMode = CameraMode.ThirdPerson;
        }
        else
        {
            currentCameraMode = CameraMode.FirstPeson;
        }
        SetCamera();
    } 

    private void SetCamera()
    {
        switch (currentCameraMode)
        {
            //if currentCameraMode.FirstPerson
            case CameraMode.FirstPeson:
                firstPersonCamera.depth = 0;
                
                thirdPersonCamera.depth = -1;
                

                break;
            
            //if currentCameraMode is CameraMode.ThirdPerson
            case CameraMode.ThirdPerson:
                thirdPersonCamera.depth = 0;
                
                firstPersonCamera.depth = -1;
                

                break;
        }
    }

    public Camera GetCurrentCamera()
    {
        if (currentCameraMode == CameraMode.FirstPeson)
        {
            return firstPersonCamera;
        }
        return thirdPersonCamera;
    }

    public CameraMode GetCameraMode()
    {
        return currentCameraMode;
    }

}
