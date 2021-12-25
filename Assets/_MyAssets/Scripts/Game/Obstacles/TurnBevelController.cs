using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBevelController : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed, 0);
    }
}
