using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchController : MonoBehaviour
{
    [SerializeField] SwitchWall switchWall;

    [Space(10)]
    [SerializeField] Transform switch_transform;
    [Space(5)]
    [SerializeField] Transform cable_vertical_transform;
    [SerializeField] Transform cable_horizon_transform;
    [SerializeField] Transform cable_sphere_transform;
    [Space(5)]
    [SerializeField] Transform lever_transform;

    [Space(10)]
    [SerializeField] Vector2 switchPosition;
    [SerializeField] float cableWidth = 1;
    [SerializeField] float switchCompleteTime_sec = 0.3f;
    [SerializeField] bool isRight = true;

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
        switch_transform.localPosition = new Vector3(switchPosition.x, 0, switchPosition.y);

        float posX = 3.9f;
        if (switchPosition.x < 0) posX *= -1;
        Vector3 pos = switch_transform.localPosition;
        pos.x = posX;
        pos.y = 0.25f;
        cable_sphere_transform.localPosition = pos;
        cable_horizon_transform.localPosition = pos;
        cable_vertical_transform.localPosition = pos;
        float length_h = (switch_transform.localPosition.x - posX) / 2;
        cable_horizon_transform.localScale = new Vector3(length_h, cableWidth, cableWidth);
        cable_vertical_transform.localScale = new Vector3(cableWidth, cableWidth, -switchPosition.y / 2);
        cable_sphere_transform.localScale = new Vector3(cableWidth / 2, cableWidth / 2, cableWidth / 2);

        if (isRight)
        {
            lever_transform.localEulerAngles = new Vector3(0, 0, -30);
        }
        else
        {
            lever_transform.localEulerAngles = new Vector3(0, 0, 30);
        }
        switchWall.SetWall(isRight);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerEnter(Collider collider)
    {
        OnTouch_Player(collider);
    }

    void OnTouch_Player(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;

        isRight = !isRight;
        if (isRight) lever_transform.DOLocalRotate(new Vector3(0, 0, -30), switchCompleteTime_sec);
        else lever_transform.DOLocalRotate(new Vector3(0, 0, 30), switchCompleteTime_sec);
        switchWall.Move(isRight);
    }
}
