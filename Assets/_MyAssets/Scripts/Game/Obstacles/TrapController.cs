using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrapController : MonoBehaviour
{
    [SerializeField] Transform tooth_right_transform;
    [SerializeField] Transform tooth_left_transform;
    [SerializeField] GameObject trapTrigger;

    [Header("閉じる時間")]
    [Space(20)]
    [SerializeField] float closeTime_sec;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        OnTouchPlayer(collider);
    }

    void OnTouchPlayer(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;
        tooth_right_transform.DOLocalRotate(new Vector3(0, 0, 90), closeTime_sec);
        tooth_left_transform.DOLocalRotate(new Vector3(0, 0, -90), closeTime_sec);
        trapTrigger.SetActive(false);
    }
}
