using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : HitScanFromCamera
{
    public Vector3 TryToGrapple()
    {
        RaycastHit hit = CastFromSceenCentre();
        if (hit.collider)
        {
            return hit.point;
        }

        return Vector3.zero;
    } 
}
