using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float verticalRotationMin, verticalRotationMax;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerEyeLevel = 0.5f;

    private float currentHorizontalRotation, currentVerticalRotation;


    // Start is called before the first frame update
    void Start()
    {

        currentHorizontalRotation = transform.localEulerAngles.y;
        currentVerticalRotation = transform.localEulerAngles.y;
             
    }

    // Update is called once per frame
    void Update()
    {

        currentHorizontalRotation += Input.GetAxis("Mouse X") * sensitivity;
        currentVerticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, verticalRotationMin, verticalRotationMax);

        transform.localEulerAngles = new Vector3
            (currentVerticalRotation, currentHorizontalRotation);
        transform.position = playerTransform.position + Vector3.up * playerEyeLevel;

    }
}
