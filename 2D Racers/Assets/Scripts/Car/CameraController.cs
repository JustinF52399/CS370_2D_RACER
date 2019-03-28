using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private Camera cam;

    public Vector3 rotation_offset;
    public Vector3 position_offset;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate(){
        LookAtTarget();
        MoveToTarget();
    }

    void LookAtTarget(){
        Quaternion rotation = Quaternion.Euler(rotation_offset.x, target.eulerAngles.y + rotation_offset.y, rotation_offset.z);
        transform.rotation = rotation;
    }

    void MoveToTarget(){
        Vector3 pos = target.position + 
                      target.forward * position_offset.z +
                      target.right * position_offset.x + 
                      target.up * position_offset.y;
        transform.position = pos;
    }
}
