using UnityEngine;
using System.Collections;

public class Laps : MonoBehaviour
{

    // These Static Variables are accessed in "checkpoint" Script
    public Transform[] checkPointArray = new Transform[4];
    public static Transform[] checkpointA ;
    public static int currentCheckpoint = 0;
    public static int currentLap = 0;
    public Vector3 startPos;
    public static int Lap;
    public static int Checkpoint;

    // sets the starting value of the checkpoint and lap to zero
    public void Start()
    {
        checkPointArray[0] = GameObject.FindWithTag("c1").transform;
        checkPointArray[1] = GameObject.FindWithTag("c2").transform;
        checkPointArray[2] = GameObject.FindWithTag("c3").transform;
        checkPointArray[3] = GameObject.FindWithTag("c4").transform;

        startPos = transform.position;
        currentCheckpoint = 0;
        currentLap = 0;

    }

    //updates the value of checkpoint u passed through and lap completed
    public void Update()
    {
        Lap = currentLap;
        Checkpoint = currentCheckpoint;
        checkpointA = checkPointArray;
    }

}
