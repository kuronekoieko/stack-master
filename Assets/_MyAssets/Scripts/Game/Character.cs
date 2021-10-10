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
    Vector3 vel;
    float currentVelocity;
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

        }

        if (Input.GetMouseButton(0))
        {
            var deltaX = Input.GetAxis("Mouse X") * 5f;
            vel.x = deltaX * speedX;
        }
        else
        {
            vel.x = 0;
        }
        vel.x = Mathf.SmoothDamp(rb.velocity.x, vel.x, ref currentVelocity, 0.1f);
        rb.velocity = vel;
    }


    void OnTriggerEnter(Collider other)
    {
        var gate = other.gameObject.GetComponent<GateController>();
        if (gate == null) return;
        gate.OnHitCharacter();
    }
}
