using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchWall : MonoBehaviour
{
    [SerializeField] Transform wall_L_transform;
    [SerializeField] Transform wall_R_transform;
    [SerializeField] Transform props_transform;

    [Space(10)]
    [SerializeField] float height_L = 5;
    [SerializeField] float height_R = 5;
    [SerializeField] float moveCompleteTime_sec = 0.5f;

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
        wall_L_transform.localScale = new Vector3(1, height_L, 1);
        wall_R_transform.localScale = new Vector3(1, height_R, 1);
        props_transform.localScale = new Vector3(1, Mathf.Max(height_L, height_R), 1);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetWall(bool isRight)
    {
        if (isRight)
        {
            wall_L_transform.localPosition = new Vector3(0, 0.25f, 0);
            wall_R_transform.localPosition = new Vector3(7.8f, 0.25f, 0);
        }
        else
        {
            wall_L_transform.localPosition = new Vector3(-7.8f, 0.25f, 0);
            wall_R_transform.localPosition = new Vector3(0, 0.25f, 0);
        }
    }

    public void Move(bool isRight)
    {
        if (isRight)
        {
            wall_L_transform.DOLocalMove(new Vector3(0, 0.25f, 0), moveCompleteTime_sec);
            wall_R_transform.DOLocalMove(new Vector3(7.8f, 0.25f, 0), moveCompleteTime_sec);
        }
        else
        {
            wall_L_transform.DOLocalMove(new Vector3(-7.8f, 0.25f, 0), moveCompleteTime_sec);
            wall_R_transform.DOLocalMove(new Vector3(0, 0.25f, 0), moveCompleteTime_sec);
        }
    }
}
