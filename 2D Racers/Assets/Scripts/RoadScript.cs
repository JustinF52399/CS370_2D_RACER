using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    public GameObject RoadPiece;
    public GameObject[] connections;
    public GameObject[] roadPiece;
    public float RedScale = 1; //Scale to reduce the length by
    public float arb; //some value to increase the angled track pieces by
    // Start is called before the first frame update
    void Start()
    {
        
        connections = GameObject.FindGameObjectsWithTag("Connect point");
        roadPiece = new GameObject[connections.Length];

        float distance;
        Vector3 p,q;
        int j = 0;
        for(int i = 0; i < connections.Length; i++)
        {
            j = i+1;
            p = connections[i].transform.position;
            if (j == connections.Length)
            {
                j = 0; //looping back to to connect from the point to point 0
            }
            q = connections[j].transform.position;
            float angle = -(((Mathf.Atan2(p.x - q.x, p.z - q.z)) * 180) / Mathf.PI)*2; //built in angle fuction wasn't working so used this           
            
            roadPiece[i] = Instantiate(RoadPiece, new Vector3((p.x + q.x) / 2, p.y + 0.1f, (p.z + q.z) / 2), new Quaternion()); // creationg a road piece objst inbetween the 2 connection points
            distance = (Vector3.Distance(connections[i].transform.position, connections[j].transform.position)) / RedScale; //Transforming the object to match between the distance of the 2 objects

            if (angle != 180 || angle != -180)
            {
                roadPiece[i].transform.localScale = new Vector3(distance + arb, 0, 5f); //special case for angled pieces
            }
            else
            {
                roadPiece[i].transform.localScale = new Vector3(distance, 0, 5f);
            }
                roadPiece[i].transform.Rotate(new Vector3(0, angle*2, 0));
            



           

        }

    }

    
    
}
