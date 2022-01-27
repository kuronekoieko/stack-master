using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PropellerController : MonoBehaviour
{
    [SerializeField] Transform body_transform;

    [Space(10)]
    [SerializeField] float rotateSpeed = 4;
    [Space(10)]
    [SerializeField] Vector3[] localPath_array;
    [SerializeField] float completeTime_sec = 1;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        body_transform.DOLocalPath(localPath_array, completeTime_sec, PathType.CatmullRom).SetEase(Ease.Linear).SetLoops(-1);
    }

    void FixedUpdate()
    {
        body_transform.Rotate(0, -rotateSpeed * Time.fixedDeltaTime / 0.02f, 0);
    }
}
