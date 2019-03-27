using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlacer : MonoBehaviour
{
    public float spacing = .1f;
    public float resolution = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] pnts = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
        foreach(Vector3 p in pnts)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = p;
            g.transform.localScale = Vector3.one *.75f;
        }
    }

    
}
