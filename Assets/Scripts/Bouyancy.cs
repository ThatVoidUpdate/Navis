using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bouyancy : MonoBehaviour
{
    public float bouyancy;
    public float Viscosity;

    public float WaterHeight;

    private Rigidbody rb;
    private BoxCollider collider;

    private Vector3[] corners = new Vector3[8];
    private List<Vector3> SunkenCorners = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        Vector3 ULFCorner = (collider.size / 2) + collider.center;
        Vector3 DRBCorner = -(collider.size / 2) + collider.center;
        corners[0] = new Vector3(ULFCorner.x, ULFCorner.y, ULFCorner.z);
        corners[1] = new Vector3(ULFCorner.x, ULFCorner.y, DRBCorner.z);
        corners[2] = new Vector3(ULFCorner.x, DRBCorner.y, ULFCorner.z);
        corners[3] = new Vector3(ULFCorner.x, DRBCorner.y, DRBCorner.z);
        corners[4] = new Vector3(DRBCorner.x, ULFCorner.y, ULFCorner.z);
        corners[5] = new Vector3(DRBCorner.x, ULFCorner.y, DRBCorner.z);
        corners[6] = new Vector3(DRBCorner.x, DRBCorner.y, ULFCorner.z);
        corners[7] = new Vector3(DRBCorner.x, DRBCorner.y, DRBCorner.z);
    }

    // Update is called once per frame
    void Update()
    {
        SunkenCorners.Clear();
        for (int i = 0; i < 8; i++)
        {
            if (transform.TransformPoint(corners[i]).y < WaterHeight)
            {
                SunkenCorners.Add(corners[i]);
            }
        }

        if (SunkenCorners.Count > 0)
        {
            Vector3 WaterCOM = new Vector3();
            for (int i = 0; i < SunkenCorners.Count; i++)
            {
                WaterCOM += SunkenCorners[i];
            }
            WaterCOM /= SunkenCorners.Count;
            Vector3 WorldWaterCOM = transform.TransformPoint(WaterCOM);

            print(Mathf.Clamp((WaterHeight - WorldWaterCOM.y) / collider.size.z * 2, 0.25f, 1));

            rb.AddForceAtPosition(bouyancy * Vector3.up * Mathf.Clamp((WaterHeight - WorldWaterCOM.y) / collider.size.z * 2, 0.25f, 1), WorldWaterCOM);
            rb.AddForceAtPosition(rb.velocity * Viscosity * -1, WorldWaterCOM);
        }
    }
}

