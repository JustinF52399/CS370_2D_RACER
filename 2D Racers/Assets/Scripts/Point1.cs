using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.SetPositionAndRotation(new Vector3(450, 0, 0), new Quaternion());
    }

}
