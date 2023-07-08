using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Transform của nhân vật cần theo dõi
    public Vector3 offset = new (0f, 5f, -10f); // Vị trí tương đối của camera so với nhân vật
    public float smoothSpeed = 0.125f; // Tốc độ di chuyển của camera

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset; // Vị trí mà camera muốn đến
        Vector3 smoothedPosition =
            Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Vị trí mới của camera được làm mượt

        transform.position = smoothedPosition; // Cập nhật vị trí của camera
        transform.LookAt(target); // Camera luôn nhìn về nhân vật
    }
}
