using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitScanFromCamera : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float maxDistance;

    private CameraSwapper cameraSwapper;
    
    // Protected = only my child can see
    // Virtual = 
    protected virtual void Start()
    {
        cameraSwapper = FindAnyObjectByType<CameraSwapper>();
    }

    protected virtual RaycastHit CastFromSceenCentre()
    {
        //get the current camera
        Camera currentCamera = cameraSwapper.GetCurrentCamera();
        // Create a ray using the centre of the screen
        Ray ray = currentCamera.ViewportPointToRay(Vector3.one * 0.5f);

        if (Physics.Raycast(ray,out RaycastHit hit, float.PositiveInfinity, hitLayer))
        {
            return hit;
        }

        return new RaycastHit();
    }

}
