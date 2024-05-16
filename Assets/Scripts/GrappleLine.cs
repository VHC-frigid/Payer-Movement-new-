using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLine : MonoBehaviour
{

    private LineRenderer line;
    
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
    }

    public void StartGrapple(Vector3 point)
    {
        line.enabled = true;
        line.SetPosition(1, point);
    }

    public void EndGrapple()
    {
        line.enabled=false;
    }
}
