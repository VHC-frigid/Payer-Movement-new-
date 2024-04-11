using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private Image crosshair;
    private CameraSwapper swapper;

    // Start is called before the first frame update
    void Start()
    {
        crosshair = GetComponent<Image>();
        swapper = FindObjectOfType<CameraSwapper>();
    }

    // Update is called once per frame
    void Update()
    {
        crosshair.enabled = Input.GetButton("Scope") || swapper.GetCameraMode() == CameraSwapper.CameraMode.FirstPeson;
    }
}
