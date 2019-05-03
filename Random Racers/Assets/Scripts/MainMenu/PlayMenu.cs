using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    public bool isEasy;
    public bool isMedium;
    public bool isHard;
    public bool isPractice;
    public bool isBack;
    public bool isHowTo;
    public bool isCredits;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseUp(){
            if(isEasy)
            {
                    SceneManager.LoadScene("Randomization");
            }
            if(isMedium)
            {
                    SceneManager.LoadScene("MediumRand");
            }
            if(isHard)
            {
                    SceneManager.LoadScene("HardRand");
            }
            if(isPractice)
            {
                    SceneManager.LoadScene("PracticeTrack");
            }
            if(isBack)
            {
                    SceneManager.LoadScene("MainMenuScene");
            }
            if(isHowTo)
            {
		    SceneManager.LoadScene("HowToPlay");
	    }
            if(isCredits)
            {
		    SceneManager.LoadScene("Credits");
	    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
