using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : HitScanFromCamera
{
    // Override = do different stuff to my pearnt
    protected override void Start()
    {

        //run my pearnt's (my base's) start method
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Scan"))
        {
            Scan();
        }
    }

    private void Scan()
    {
        RaycastHit hit = CastFromSceenCentre();
        // If we hit something, and hit has the Scannable script...
        if (hit.collider && hit.collider.TryGetComponent<Scannable>(out Scannable scannedObject))
        {
            scannedObject.Scan();
        }
    }

}
