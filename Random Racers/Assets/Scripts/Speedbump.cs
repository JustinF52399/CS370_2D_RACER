using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedbump : MonoBehaviour
{
    Vector3 startingspeed;
    Vector3 newspeed;
    Rigidbody rb;

    void OnTriggerEnter(Collider collider)
    {
        rb = collider.GetComponent<Rigidbody>();
        startingspeed = rb.velocity;
        newspeed = startingspeed * 0.4f;
        rb.velocity = Vector3.zero;
    }

    void OnTriggerExit(Collider collider)
    {
        rb.velocity = startingspeed;
    }

    private void OnTriggerStay(Collider collider)
    {
        print(rb.velocity);
        rb.velocity = newspeed;
    }
}
