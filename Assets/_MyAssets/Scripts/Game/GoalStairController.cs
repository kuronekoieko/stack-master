using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalStairController : MonoBehaviour
{

    [SerializeField] MeshRenderer stepMr;
    [SerializeField] MeshRenderer stepBaseMr;
    public float StepHeight => stepMr.transform.localScale.y;
    public float StepDepth => stepMr.transform.localScale.z;


    public void OnInstansiate(Vector3 pos, float height, Color stepColor)
    {
        transform.position = pos;
        stepMr.material.color = stepColor;
        stepBaseMr.transform.localPosition -= Vector3.up * (height + StepHeight / 2f) / 2f;
        Vector3 scale = stepBaseMr.transform.localScale;
        scale.y = height;
        stepBaseMr.transform.localScale = scale;
    }
}
