using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text timerText;
    public static float startTime;
    public static string minutes;
    public static string seconds;
    public float c_time;
    // Start is called before the first frame update

    void Start()
    {
        timerText = GameObject.FindWithTag("t_text").GetComponent<Text>();
        startTime = Time.time;
    }

    // Update is called once per frame
    // Converts the time to minutes and seconds
    void Update()
    {
         c_time = Time.time - startTime;
         minutes = ((int)c_time / 60).ToString();
         seconds = (c_time % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds;
    }

    
}
