using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    new public Camera camera;
    public GameObject Target;
    public Vector3 TargetOffset;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = Target.transform.position + TargetOffset - camera.transform.position;
        camera.transform.position = camera.transform.position + offset / Speed;


        camera.transform.LookAt(Target.transform);
    }
}
