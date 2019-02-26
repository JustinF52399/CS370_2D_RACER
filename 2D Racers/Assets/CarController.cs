using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 5;
    public float minSpeed = -1;
    public float currentSpeed = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float turn = Input.GetAxis("Horizontal");
        float speed = Input.GetAxis("Vertical");
        float nextSpeed;

        if (speed < 0 && currentSpeed > 0) //Braking
        {
            nextSpeed = this.currentSpeed + (3 * speed * Time.deltaTime);
        }
        else if (speed == 0) //Coast
        {
            if (currentSpeed > 0.05)
            {
                nextSpeed = this.currentSpeed - Time.deltaTime;
            }
            else if (currentSpeed < -0.05)
            {
                nextSpeed = this.currentSpeed + Time.deltaTime;
            }
            else
            {
                nextSpeed = 0;
            }
        }
        else // Acceleration
        {
            nextSpeed = this.currentSpeed + (speed * Time.deltaTime);
        }

        if (minSpeed <= nextSpeed && nextSpeed <= maxSpeed)
        {
            this.currentSpeed = nextSpeed;
        }
        transform.Translate(currentSpeed, 0f, 0f);

        if (this.currentSpeed != 0)
        {
            transform.Rotate(0f, turn * 90 * Time.deltaTime, 0f);
        }
    }
}