using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoor : MonoBehaviour
{
    [SerializeField] private float height = 2f;

    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z = _startPosition.z + Mathf.PingPong(Time.time, height);
        transform.position = pos;
    }
}
