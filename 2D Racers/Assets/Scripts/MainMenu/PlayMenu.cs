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
                    Application.LoadLevel(3);
            }
            if(isHard)
            {
                    Application.LoadLevel(4);
            }
            if(isPractice)
            {
                    SceneManager.LoadScene("PracticeTrack");
            }
            if(isBack)
            {
                    Application.LoadLevel(0);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
