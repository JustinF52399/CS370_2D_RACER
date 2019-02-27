using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 500;
    public float minSpeed = -100;

    public Rigidbody car_rb;


    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation;
        car_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float turn = Input.GetAxis("Horizontal");
        if (car_rb.velocity.magnitude != 0)
        {
            targetRotation *= Quaternion.AngleAxis(100 * turn * Time.deltaTime, Vector3.up);
            car_rb.rotation = targetRotation;
        }
    }

    void FixedUpdate()
    {
        float speed = Input.GetAxis("Vertical");
        Vector3 nextVel = car_rb.velocity + (transform.forward * Mathf.RoundToInt(speed) * 50);
        print(nextVel.magnitude);
        if (nextVel.magnitude > minSpeed && nextVel.magnitude < maxSpeed)
        {
            car_rb.velocity += nextVel;
        }
    }
}