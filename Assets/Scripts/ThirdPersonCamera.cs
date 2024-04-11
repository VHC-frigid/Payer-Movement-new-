using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //control how fast the camera looks around
    [SerializeField] private float sensitivity;

    //keep track of the player's position
    [SerializeField] private Transform playerTransform;

    //prevent the camera from rotating too far up or down
    [SerializeField] private float verticalRotationMin = 0, verticalRotationMax = 75;

    //the ideal value for the camera zoom
    [SerializeField] private float cameraZoom = 20;
    //the layermask to look for when avoiding objects
    [SerializeField] private LayerMask avoidLayer;

    [SerializeField] private float scopeZoomSpeed;

    //track our ideal and our actual camera zoom
    private float idealCameraZoom;
    private float currentCameraZoom;

    //keep track of our different camera parts
    private Transform cameraTransform, boomTransform;

    //track our current rotation values
    private float currentHorizontalRotation, currentVerticalRotation;

    // Start is called before the first frame update
    void Start()
    {
        //save both parts of our camera
        boomTransform = transform.GetChild(0);
        cameraTransform = boomTransform.GetChild(0);

        //set our initial rotation values
        currentHorizontalRotation = transform.localEulerAngles.y;
        currentVerticalRotation = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.currentState != GameState.Play)
            return;
        //rotate left to right based on mouse movement
        currentHorizontalRotation += Input.GetAxis("Mouse X") * sensitivity;
        //rotate up and down based on mouse movement
        currentVerticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;

        //Clamp our vertical rotation
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, verticalRotationMin, verticalRotationMax);

        //set the new rotation values
        transform.localEulerAngles = new Vector3(0, currentHorizontalRotation);
        boomTransform.localEulerAngles = new Vector3(currentVerticalRotation, 0);
        //make the camera snap to the player
        transform.position = playerTransform.position;

        //direction from A to B = B - A
        Vector3 directionToCamera = cameraTransform.position - playerTransform.position;
        //normalise to make sure we have a magnitude of 1
        directionToCamera.Normalize();

        if (Physics.Raycast(transform.position,directionToCamera, out RaycastHit hit,cameraZoom, avoidLayer))
        {
            currentCameraZoom = hit.distance;
        }
        else
        {
            currentCameraZoom = cameraZoom;
        }
        Vector3 cameraTargetPosition = new Vector3();

        if (Input.GetButton("Scope"))
        {
            cameraTargetPosition = Vector3.MoveTowards(cameraTransform.localPosition,new Vector3(1,0, -cameraZoom/10), scopeZoomSpeed * Time.deltaTime);
        }
        else
        {
            cameraTargetPosition = Vector3.MoveTowards(cameraTransform.localPosition,new (0, 0, -currentCameraZoom), scopeZoomSpeed *Time.deltaTime);
        }

        cameraTransform.localPosition = cameraTargetPosition;
        
    }
}
