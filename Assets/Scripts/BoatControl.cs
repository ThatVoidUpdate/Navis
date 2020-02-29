using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatControl : MonoBehaviour
{
    private Rigidbody rb;
    public float Speed;
    public GameObject Propeller;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForceAtPosition(Vector3.forward * Speed, transform.InverseTransformPoint(Propeller.transform.position));
        }
    }
}
