using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private LayerMask floorMask;
    public PlayerController player;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            // mousePosition.z = 10;
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            if (Physics.Raycast(ray, out RaycastHit hit, floorMask))
            {
                if (hit.transform.CompareTag("Floor"))
                {
                    var pos = hit.point;
                    pos.y += 0.001f;
                    crosshair.transform.position = hit.point;
                }
            }
        }
    }
}
