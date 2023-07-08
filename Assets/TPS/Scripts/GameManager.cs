using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;
    [SerializeField] private LayerMask floorMask;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            var pos = Camera.main.ScreenToWorldPoint(mousePosition);
            crosshair.transform.position = pos;
            // Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            // GameObject floor;
            // // var hit = Physics.Raycast(ray, out floor, floorMask);
            // if (Physics.Raycast(ray, out RaycastHit hit, floorMask))
            // {
            //     Debug.Log(hit.transform.name);
            //     if (hit.transform.CompareTag("Floor"))
            //     {
            //         crosshair.transform.position = hit.transform.position;
            //     }
            // }
        }
    }
}
