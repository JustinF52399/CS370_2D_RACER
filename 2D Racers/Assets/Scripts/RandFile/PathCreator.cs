using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{

    
    public Path path;
    public Color anchorCol = Color.red;
    public Color controlColor = Color.white;
    public Color segmentColor = Color.green;
    public Color selectedSegmentColor = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = .075f;
    public bool displayControlPoints = true;


    public void CreatePath(Vector3 p1, Vector3 p2)
    {
        Debug.Log("Creator P1: " + p1 + " P2" + p2 );
        path = new Path(p1,p2);
    }

}
