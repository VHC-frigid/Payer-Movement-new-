using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Firstpersoncontroller : CustomController
{
    [SerializeField] private Transform cameraTransform;

    protected override void Move(Vector3 direction)
    {
        transform.localEulerAngles = new Vector3(0, cameraTransform.localEulerAngles.y);
        direction = transform.TransformDirection(direction);
        base.Move(direction);
    }

}
