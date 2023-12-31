using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new(0f, 5f, -10f);
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        // Vector3 smoothedPosition =
        //     Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}
