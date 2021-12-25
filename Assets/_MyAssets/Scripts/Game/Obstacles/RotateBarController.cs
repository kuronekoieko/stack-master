using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBarController : MonoBehaviour
{
    [Header("回転速度")]
    [SerializeField] float rotateSpeed = 1;

    [Header("上下移動速度")]
    [Space(10)]
    [SerializeField] float moveSpeed_y = 0;
    [Header("上下移動範囲")]
    [SerializeField] float moveRange_y = 0;

    [Header("前後移動速度")]
    [Space(10)]
    [SerializeField] float moveSpeed_z = 0;
    [Header("前後移動範囲")]
    [SerializeField] float moveRange_z = 0;

    Vector3 firstPosition;
    bool isUp;
    bool isForward;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        firstPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed * Time.fixedDeltaTime / 0.02f, 0);
        MoveY();
        MoveZ();
    }

    void MoveY()
    {
        if (transform.position.y > firstPosition.y + moveRange_y / 2)
        {
            Vector3 pos = transform.position;
            pos.y = firstPosition.y + moveRange_y / 2;
            transform.position = pos;
            isUp = false;
        }
        else if (transform.position.y < firstPosition.y - moveRange_y / 2)
        {
            Vector3 pos = transform.position;
            pos.y = firstPosition.y - moveRange_y / 2;
            transform.position = pos;
            isUp = true;
        }

        if (isUp) transform.position += new Vector3(0, moveSpeed_y * Time.fixedDeltaTime / 0.02f, 0);
        else transform.position -= new Vector3(0, moveSpeed_y * Time.fixedDeltaTime / 0.02f, 0);
    }

    void MoveZ()
    {
        if (transform.position.z > firstPosition.z + moveRange_z / 2)
        {
            Vector3 pos = transform.position;
            pos.z = firstPosition.z + moveRange_z / 2;
            transform.position = pos;
            isForward = false;
        }
        else if (transform.position.z < firstPosition.z - moveRange_z / 2)
        {
            Vector3 pos = transform.position;
            pos.z = firstPosition.z - moveRange_z / 2;
            transform.position = pos;
            isForward = true;
        }

        if (isForward) transform.position += new Vector3(0, 0, moveSpeed_z * Time.fixedDeltaTime / 0.02f);
        else transform.position -= new Vector3(0, 0, moveSpeed_z * Time.fixedDeltaTime / 0.02f);
    }
}