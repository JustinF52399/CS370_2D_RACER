using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.SetPositionAndRotation(new Vector3(350, 0, 350), new Quaternion());
    }

}
