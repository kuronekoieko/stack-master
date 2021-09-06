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

    void Start()
    {
        cameraController.Target = transform;
    }


    void Update()
    {
        var vel = rb.velocity;
        vel.z = speedZ;

        if (Input.GetMouseButton(0))
        {
            vel.x = Input.GetAxis("Mouse X") * speedX;
        }

        if (Input.GetMouseButtonUp(0))
        {
            vel.x = 0;
        }
        rb.velocity = vel;
    }




}
