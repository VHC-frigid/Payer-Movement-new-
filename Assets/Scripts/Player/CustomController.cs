using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomController : CombatAgent
{
    public enum State
    {

        Idel,
        Walk,
        Jump,
        Grapple

    }

    [SerializeField] private float staminaCurrent, staminaMax = 100, staminaRunCost = 10, staminaRechargeRate = 20, staminaRechargeDelay = 1.5f;
    private float staminaTimeLastUsed;

    [SerializeField] private State currentState;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float gravity;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float maxGrappleSpeed = 80;

    private float currentGrappleSpeed;

    private Vector3 grapplePoint = new Vector3();
    //how close to the ground we need to be, to be "grounded"
    [SerializeField] private float groudedAllowance;
    //the angle in degrees we're allowed to walk up
    [SerializeField] private float walkAngle = 40f;
    //[SerializeField] private Transform cameraTransform;
    private Rigidbody rb;

    private CameraSwapper cameraSwapper;

    private GrappleLine grappleRenderer;

    //hold on to our momentum values
    private float horizontalSpeed, verticalSpeed;

    private Vector2 inputThisFrame = new Vector2();
    private Vector3 movementThisFrame = new Vector3();

    private PlayerUi playerUI;

    public GameObject GOPanel;

    // Start is called before the first frame update
    protected override void Start()
    {
        //do the Stat behaviour from my pearnt
        base.Start();
        rb = GetComponent<Rigidbody>();
        cameraSwapper = GetComponent<CameraSwapper>();
        grappleRenderer = GetComponentInChildren<GrappleLine>();
        playerUI = FindObjectOfType<PlayerUi>();
        staminaCurrent = staminaMax;
        playerUI.SetNewSize(healthMax);
        NextState();
    }

    private void NextState()
    {

        switch (currentState)
        {
            case State.Idel:
                StartCoroutine("IdleState");
                break;
            case State.Walk:
                StartCoroutine("WalkState");
                break;
            case State.Jump:
                StartCoroutine("JumpState");
                break;
            case State.Grapple:
                StartCoroutine("GrappleState");
                break;
        }

    }

    private void ChangeState(State newState)
    {
        currentState = newState;
    }

    private void AscendAt(float speed)
    {
        verticalSpeed = speed;
        ChangeState(State.Jump);
    }

    #region State Coroutines
    IEnumerator IdleState()
    {
        //Enter idle
        Move(Vector3.zero);
        while (currentState == State.Idel)
        {
            //while in Idle
            if (!IsGrounded())
            {
                ChangeState(State.Jump);
            }
            else
            {
                if (inputThisFrame.magnitude != 0)
                {
                    ChangeState(State.Walk);
                }
                if (Input.GetButton("Jump"))
                {
                    AscendAt(jumpPower);
                }
            }
            yield return null;
        }
        //Exit Idle

        NextState();
    }
    IEnumerator WalkState()
    {

        while (currentState == State.Walk)
        {
            movementThisFrame = new();

            movementThisFrame.x = inputThisFrame.x;
            movementThisFrame.z = inputThisFrame.y;

            float speedThisFrame = walkSpeed;

            if (Input.GetButton("Sprint") && TryToUseStamina(staminaRunCost * Time.deltaTime))
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

            if (IsGrounded())
            {
                if (Input.GetButton("Jump"))
                {
                    AscendAt(jumpPower);
                }
            }
            else
            {
                ChangeState(State.Jump);
            }

            Move(movementThisFrame);

            yield return null;
        }

        NextState();
    }
    IEnumerator JumpState()
    {

        while (currentState == State.Jump)
        {

            movementThisFrame = new Vector3(inputThisFrame.x, 0, inputThisFrame.y);
            movementThisFrame *= horizontalSpeed;

            movementThisFrame = TransformDirection(movementThisFrame);

            verticalSpeed -= gravity * Time.deltaTime;
            movementThisFrame.y = verticalSpeed;

            Move(movementThisFrame);

            if(IsGrounded() && verticalSpeed < 0)
            {
                ChangeState(State.Walk);
            }

            yield return null;
        }

        NextState();
    }
    IEnumerator GrappleState()
    {

        grappleRenderer.StartGrapple(grapplePoint);
        Vector3 grappleDirecion = grapplePoint - transform.position;
        grappleDirecion.Normalize();
        while (currentState == State.Grapple)
        {
            currentGrappleSpeed = Mathf.Clamp(currentGrappleSpeed + maxGrappleSpeed * Time.deltaTime, 0, maxGrappleSpeed);

            if (Vector3.Distance(grapplePoint, transform.position) < 2f || Input.GetButtonUp("Grapple"))
            {
                EndGrapple();
            }
            else
            {
                Move(grappleDirecion * currentGrappleSpeed);
            }

            yield return null;
        }
        grappleRenderer.EndGrapple();
        NextState();
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        inputThisFrame = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputThisFrame.Normalize(); //changes inputThisFrame into a normalized vector
        //inputThisFrame.normalized - gives us the normalized vector, without changing the vector itself  
        if (Input.GetButtonDown("Grapple"))
        {
            TryToGrapple();
        }
        //if the current time has surpased the last used time and the delay...
        if (Time.time > staminaTimeLastUsed + staminaRechargeDelay)
        {
            staminaCurrent = Mathf.Clamp(staminaCurrent + staminaRechargeRate * Time.deltaTime, 0, staminaMax);
        }

        // current/max = a percent of max, between 0 and 1
        playerUI.UpdateHUD(healthCurrent / healthMax, staminaCurrent / staminaMax);
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

    protected override void EndOfLife()
    {
        //put gameover behaviour in here
        GOPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    #region Grapple methods

    private void TryToGrapple()
    {
        grapplePoint = GetComponent<Grapple>().TryToGrapple();
        if (grapplePoint != Vector3.zero)
        {
            StartGrapple();
        }
    }

    private void StartGrapple()
    {
        verticalSpeed = 0;
        horizontalSpeed = 0;
        ChangeState(State.Grapple);
    }

    private Vector3 externalVelocity = Vector3.zero;

    private void EndGrapple()
    {
        grapplePoint = Vector3.zero;
        currentGrappleSpeed = 0;
        externalVelocity = rb.velocity;
        //rb.velocity = Vector3.zero;
        //ChangeState(State.Idel);
        ChangeState(State.Jump);
    }

    #endregion

    public bool TryToUseStamina(float cost)
    {
        if(staminaCurrent == 0)
        {
            return false;
        }

        staminaCurrent = Mathf.Clamp(staminaCurrent - cost, 0, staminaMax);
        staminaTimeLastUsed = Time.time;
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Item>(out Item item))
        {
            switch (item.ItemType)
            {
                case Item.Type.Heal:
                    Heal(0.2f);
                    break;
                case Item.Type.UpgradeHealth:
                    healthMax += 3f;
                    healthCurrent = healthMax;
                    playerUI.SetNewSize(healthMax);
                    break;
                case Item.Type.UpgradeGun:
                    break;
                case Item.Type.UpgradeStamina:
                    break;
                case Item.Type.UpgradeGrapple:
                    break;
            }
            Destroy(item.gameObject);
        }
    }
}
