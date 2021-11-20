using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalStairController : MonoBehaviour
{

    [SerializeField] MeshRenderer stepMr;
    public float StepHeight => stepMr.transform.localScale.y;
    public float StepDepth => stepMr.transform.localScale.z;




    public void OnInstansiate(float height, Color stepColor)
    {
        stepMr.material = new Material(stepMr.material);
        stepMr.material.color = stepColor;
    }
}
