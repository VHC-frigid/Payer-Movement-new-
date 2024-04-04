using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float gravity;
    [SerializeField] private LayerMask groundMask;
    //how close to the ground we need to be, to be "grounded"
    [SerializeField] private float groudedAllowance;
    //the angle in degrees we're allowed to walk up
    [SerializeField] private float walkAngle = 40f;
    //[SerializeField] private Transform cameraTransform;
    private Rigidbody rb;

    private CameraSwapper cameraSwapper;

    //hold on to our momentum values
    private float horizontalSpeed, verticalSpeed;

    private Vector2 inputThisFrame = new Vector2();
    private Vector3 movementThisFrame = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraSwapper = GetComponent<CameraSwapper>();
    }

    // Update is called once per frame
    void Update()
    {
        inputThisFrame = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputThisFrame.Normalize(); //changes inputThisFrame into a normalized vector
        //inputThisFrame.normalized - gives us the normalized vector, without changing the vector itself
        
        movementThisFrame = new();

        movementThisFrame.x = inputThisFrame.x;
        movementThisFrame.z = inputThisFrame.y;

        float speedThisFrame = walkSpeed;

        if (Input.GetButton("Sprint"))
        {
            speedThisFrame = runSpeed;
        }
        if (Input.GetButton("Crouch"))
        {
            speedThisFrame = crouchSpeed;
        }
        
        movementThisFrame = TransformDirection(movementThisFrame);

        if (inputThisFrame.magnitude > 0 && ValidateDirection(movementThisFrame))
        {
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, speedThisFrame, runSpeed * Time.deltaTime);
        }
        else
        {
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0, runSpeed * Time.deltaTime);
        }

        movementThisFrame *= horizontalSpeed;

        verticalSpeed -= gravity * Time.deltaTime;

        if (IsGrounded())
        {
            verticalSpeed = Mathf.Clamp(verticalSpeed, 0, float.PositiveInfinity);
            if (Input.GetButton("Jump"))
            {
                verticalSpeed = jumpPower;
            }
        }

        movementThisFrame.y = verticalSpeed;

        Move(movementThisFrame);     
    }

    protected virtual void Move(Vector3 direction)
    {
        rb.velocity = direction;
    }

    private bool IsGrounded()
    {
        
        //cas the bottom o our collider downward
        if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit, (1/2f) + groudedAllowance, groundMask))
        {
            //if we find ground ...
            //check if the ground is flat 
            //if so ,we;re grounded
            return ValidateGroundAngle(hit.normal);
        }
        return false;
        //else we're not grounded
        
    }
        //old code
        //return Physics.Raycast(transform.position, Vector3.down, 1.001f, groundedMask);

    //transform our input direction into our local direction
    private Vector3 TransformDirection(Vector3 direction)
    {
        //get which camera is currently active
        if (cameraSwapper.GetCameraMode() == CameraSwapper.CameraMode.FirstPeson)
        {
            //if it's our first person camera..
            //make sure we're facing the same way as the camera
            FaceDirection(cameraSwapper.GetCurrentCamera().transform.localEulerAngles);
            //translate based on our transform
            return transform.TransformDirection(direction);
        }
        //otherise, transform based on the CameraAnachor transform
        return cameraSwapper.GetCurrentCamera().transform.root.TransformDirection(direction);
    }

    //make us face the way we're supposed to face
    private void FaceDirection(Vector3 direction)
    {
        //get which camera is currently active
        if (cameraSwapper.GetCameraMode() == CameraSwapper.CameraMode.FirstPeson)
        {
            //if it's our fir person camer...
            //snapping our player's y rotation to match the camera
            transform.localEulerAngles = new Vector3(0, direction.y, 0);
        }
        //otherwise, we want to use our movement diretion
        else
        {
            //use our movement, but don't roatate upwards/downwards
            transform.forward = new Vector3(direction.x, 0, direction.z);
        }
    }
    
    //check if the gound we're trying to alk on is valid (i.e. nt too steep)
    private bool ValidateDirection(Vector3 direction)
    {
        //we want to check where we're about to be (some kind of cast)
        if (Physics.SphereCast(transform.position + Vector3.down * 0.5f, 0.5f, direction, out RaycastHit hit, 0.5f, groundMask))
        {
            //if we find grond there...
            //check if it's flat
            return ValidateGroundAngle(hit.normal);
        }
        //if we don't find ground, we're allowed to move      
        return true;
    }

    //check if the certain ground is flat
    private bool ValidateGroundAngle(Vector3 groundNormal)
    {
        //compare the angl of the ground, to our walkable angle
        //if the ground is too step, the check fails
        if (Vector3.Angle(Vector3.up, groundNormal) < walkAngle)
        {
            return true;
        }
        return false;
    }


}
