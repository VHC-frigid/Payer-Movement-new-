using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : CustomController
{
    //hold the transform of the camera anchor
    [SerializeField] private Transform cameraTransform;

    protected override void Move(Vector3 direction)
    {
        //take the input direction, and transform it based on the camera facing direction
        direction = cameraTransform.TransformDirection(direction);
        //if the player is pressing a direction
        if (direction.magnitude > 0.5f)
        {
            //determine our facing direction ignoring Y movement
            Vector3 facingDirection = new Vector3(direction.x, 0, direction.z);
            //apply the direction as our forward 
            transform.forward = facingDirection;
        }
        
        base.Move(direction);
    }
}
