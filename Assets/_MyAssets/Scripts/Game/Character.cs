using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Character : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [Inject] CameraController cameraController;
    float speedZ = 10f;
    float speedX = 15f;
    // Vector3 preMousePosition;
    Vector3 vel;
    void Start()
    {
        cameraController.Target = transform;
    }


    void Update()
    {
        vel = rb.velocity;
        vel.z = speedZ;

        if (Input.GetMouseButtonDown(0))
        {
            // preMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            //  var deltaX = Input.mousePosition.x - preMousePosition.x;
            //  preMousePosition = Input.mousePosition;

            var deltaX = Input.GetAxis("Mouse X") * 5f;
            //  Debug.Log(deltaX);
            vel.x = deltaX * speedX;
        }
        else
        {
            vel.x = 0;
        }
        rb.velocity = vel;
    }
}
