using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public Text checkpoint_lap;
    public int currentLap;

    public Text lap_time;
    public int counter = 0;
    
    //displays the values stored in  the lap class
    void Start()
    {
        currentLap = Laps.currentLap;
        lap_time = GameObject.FindWithTag("l_text").GetComponent<Text>();
        checkpoint_lap = GameObject.FindWithTag("c_text").GetComponent<Text>();
        checkpoint_lap.text = "checkpoint: " + Laps.currentCheckpoint + "/4  Lap: " + Laps.currentLap + "/1";
       lap_time.text = " ";
    }

    // method that runs when car goes through a checkpoint
    void OnTriggerEnter(Collider other)
    {
        //Is it the Player who enters the collider?
        if (!other.CompareTag("Player"))
            return; //If it's not the player dont continue


        if (transform == Laps.checkpointA[Laps.currentCheckpoint].transform)
        {
            //Check so we dont exceed our checkpoint quantity
            if (Laps.currentCheckpoint + 1 < Laps.checkpointA.Length)
            {
                //Add to currentLap if currentCheckpoint is 0, which happens once you go through all the laps
                if (Laps.currentCheckpoint == 0)
                {
                    Debug.Log(counter);
                    counter++;
                    Debug.Log("AAA");
                    if (counter > 1)
                    {
                        Laps.currentLap++;
                        lap_time.text = "Lap time: " + Timer.minutes + ":" + Timer.seconds;
                        currentLap = Laps.currentLap;
                    }
                    
                    
                }
                Laps.currentCheckpoint++;

            }
            else
            {
                //If we dont have any Checkpoints left, go back to 0
                Laps.currentCheckpoint = 0;
                Debug.Log(counter);
                counter++;
            }
            checkpoint_lap.text = "checkpoint: " + Laps.currentCheckpoint + "/4  Lap: " + (Laps.currentLap) + "/1";
        }
        

    }

}
