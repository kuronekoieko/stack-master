using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollapseWallController : MonoBehaviour
{
    [SerializeField] Transform wall_transform;
    [SerializeField] Transform stage_transform;
    [SerializeField] Transform stageWall_transform;
    [SerializeField] Transform[] triggers;

    [Space(10)]
    [SerializeField] float width = 1;
    [SerializeField, Range(0.1f, 7.5f)] float height = 3;
    [SerializeField] float collapseCompleteTime_sec = 1;
    [SerializeField] AnimationCurve collapse_animationCurve;

    bool isFirstCall = true;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnValidate()
    {
        stage_transform.localScale = new Vector3(2, 1, width * 2);
        stageWall_transform.localScale = new Vector3(1, height, width);
        wall_transform.localScale = new Vector3(1, height / 2, width);
        triggers[0].transform.localPosition = new Vector3(0, 0, -width - 2);
        triggers[1].transform.localPosition = new Vector3(0, 0, width + 2);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerEnter(Collider collider)
    {
        OnTouchPlayer(collider);
    }

    void OnTouchPlayer(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;
        if (!isFirstCall) return;

        wall_transform.DOLocalRotate(new Vector3(0, 0, -90), collapseCompleteTime_sec).SetEase(collapse_animationCurve);
        isFirstCall = false;
    }
}