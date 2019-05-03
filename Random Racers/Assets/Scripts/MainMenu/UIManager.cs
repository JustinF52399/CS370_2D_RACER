using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameObject[] pauseObjects;
    GameObject[] winObjects;
    GameObject[] loseObjects;
    public int Laps;
    public string difficulty;

    // Use this for initialization
    void Start()
    {
        difficulty = SceneManager.GetActiveScene().name;
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
        winObjects = GameObject.FindGameObjectsWithTag("ShowOnWin");        
        hideWin();
        loseObjects = GameObject.FindGameObjectsWithTag("ShowOnLose");
        hideLose();

    }

    // Update is called once per frame
    void Update()
    {

        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                hidePaused();
            }
        }
        
        Laps = GameObject.FindGameObjectWithTag("c1").GetComponent<Checkpoint>().currentLap;
        

        //Shows win menu once lap is complete
        if (Laps == 1)
        {
            
            Time.timeScale = 0;
            if(difficulty == "PracticeTrack")
            {
                
              showWin();
                
            }
            if (difficulty == "Randomization")
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Timer>().c_time < 90)
                {
                    showWin();
                }
                else
                {
                    showLose();
                }

            }
            else if (difficulty == "MediumRand")
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Timer>().c_time < 300)
                {
                    showWin();
                }
                else
                {
                    showLose();
                }
            }
            else if (difficulty == "HardRand")
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Timer>().c_time < 540)
                {
                    showWin();
                }
                else
                {
                    showLose();
                }
            }
        }
    }


    //Reloads the Level
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach (GameObject x in pauseObjects)
        {
            x.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach (GameObject x in pauseObjects)
        {
            x.SetActive(false);
        }
    }
    public void hideLose()
    {
        foreach(GameObject x in loseObjects)
        {
            x.SetActive(false);
        }
    }
    public void showLose()
    {
        foreach (GameObject x in loseObjects)
        {
            x.SetActive(true);
        }
    }

    //shows objects with ShowOnWin tag
    public void showWin()
    {
        foreach (GameObject x in winObjects)
        {
            x.SetActive(true);
        }
    }

    //hides objects with ShowOnWin tag
    public void hideWin()
    {
        foreach (GameObject x in winObjects)
        {
            x.SetActive(false);
        }
    }

    //loads inputted level
    public void LoadLevel(string level)
    {
        Application.LoadLevel(level);
    }
}
