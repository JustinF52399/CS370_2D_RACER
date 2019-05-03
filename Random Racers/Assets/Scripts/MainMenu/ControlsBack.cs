using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsBack : MonoBehaviour
{
    
    public bool isBack;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseUp(){
            if(isBack)
            {
                    SceneManager.LoadScene("MainMenuScenePlay");
            }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
