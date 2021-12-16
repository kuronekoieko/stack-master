using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBarController : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1;
    Rigidbody _rigidbody;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        //_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed, 0);
        //transform.position += new Vector3(0, 0, -rotateSpeed);
        //_rigidbody.AddTorque(0, 1000, 0);
    }
}
