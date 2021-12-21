using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] Transform muzzle_transform;
    [SerializeField] Rigidbody cannonball_rigidbody_original;
    [SerializeField] ParticleSystem smoke_particleSystem;

    [Space(10)]
    [SerializeField] float shootPower;
    [SerializeField] float shootTime_sec;

    float latestShootTime_sec;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Time.fixedTime - latestShootTime_sec < shootTime_sec) return;

        latestShootTime_sec = Time.fixedTime;
        smoke_particleSystem.Stop();
        smoke_particleSystem.Play();
        Rigidbody cannonball_rigidbody_clone = Instantiate(cannonball_rigidbody_original);
        cannonball_rigidbody_clone.transform.position = muzzle_transform.position;
        cannonball_rigidbody_clone.AddForce(-transform.right * shootPower);
    }
}
