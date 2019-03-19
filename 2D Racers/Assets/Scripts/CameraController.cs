using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private Camera cam;

    public Vector3 offset = Vector3.zero;

    public Vector3 currentPos = new Vector3(0f, 10f, 3f);
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate(){
        LookAtTarget();
        MoveToTarget();
    }

    void LookAtTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 2.5f * Time.deltaTime);
    }

    void MoveToTarget(){
        Vector3 pos = target.position + 
                      target.forward * offset.z +
                      target.right * offset.x + 
                      target.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, pos, 2.5f * Time.deltaTime);
    }
}
