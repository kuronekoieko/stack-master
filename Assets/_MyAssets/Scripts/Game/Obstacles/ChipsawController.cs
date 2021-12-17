using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipsawController : MonoBehaviour
{
    [SerializeField] Transform saw_transform;

    [Header("移動速度")]
    [Space(10)]
    [SerializeField] float moveSpeed = 1;
    [Header("回転速度")]
    [SerializeField] float rotateSpeed = 1;

    [Header("移動幅")]
    [Space(10)]
    [SerializeField] float moveWidth = 7;

    bool isRightMove = false;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void FixedUpdate()
    {
        MoveSaw();
        RotateSaw();
    }

    void MoveSaw()
    {
        if (saw_transform.localPosition.x < -moveWidth / 2)//左端に着いたら方向転換
        {
            isRightMove = true;
            Vector3 pos = saw_transform.localPosition;
            pos.x = -moveWidth / 2;
            Debug.Log($"left {pos}");
            saw_transform.localPosition = pos;
        }
        else if (saw_transform.localPosition.x > moveWidth / 2)//右端に着いたら方向転換
        {
            isRightMove = false;
            Vector3 pos = saw_transform.localPosition;
            pos.x = moveWidth / 2;
            Debug.Log($"right {pos}");
            saw_transform.localPosition = pos;
        }

        if (isRightMove) saw_transform.localPosition += new Vector3(moveSpeed * Time.fixedDeltaTime, 0, 0);
        else saw_transform.localPosition -= new Vector3(moveSpeed * Time.fixedDeltaTime, 0, 0);
    }

    void RotateSaw()
    {
        saw_transform.Rotate(0, -rotateSpeed * Time.fixedDeltaTime * 100, 0);
    }
}
