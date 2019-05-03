using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    private Camera cam;

    public Vector3 rotation_offset = new Vector3(15f, 0f, 0f);
    public Vector3 position_offset = new Vector3(0f, 3f, -10f);

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        target = GameObject.FindWithTag("Player").transform;
    }
    //looks at player
    private void LateUpdate(){
        LookAtTarget();
        MoveToTarget();
    }
    //looks at player
    void LookAtTarget(){
        Quaternion rotation = Quaternion.Euler(rotation_offset.x, target.eulerAngles.y + rotation_offset.y, rotation_offset.z);
        transform.rotation = rotation;
    }
    //follows player
    void MoveToTarget(){
        Vector3 pos = target.position + 
                      target.forward * position_offset.z +
                      target.right * position_offset.x + 
                      target.up * position_offset.y;
        transform.position = pos;
    }
}
