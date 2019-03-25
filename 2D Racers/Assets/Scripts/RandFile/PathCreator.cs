using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{

    
    public Path path;


    public void CreatePath(Vector3 p1, Vector3 p2)
    {
        Debug.Log("Creator P1: " + p1 + " P2" + p2 );
        path = new Path(p1,p2);
    }

}
