using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Vector3 rotationPoint;
    public float touchDownDistance = 0;
    public float rotationSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(rotationPoint);
        transform.RotateAround(rotationPoint, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
