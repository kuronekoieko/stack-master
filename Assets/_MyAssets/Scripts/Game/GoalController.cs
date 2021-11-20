using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalController : MonoBehaviour
{
    [SerializeField] GoalStairController firstGoalStair;
    [SerializeField] Color[] stepColors;
    GoalStairController[] goalStairs;

    float stepHeight;
    float stepDepth;


    void Awake()
    {
        int stepCount = 50;
        goalStairs = new GoalStairController[stepCount];
        Vector3 pos = firstGoalStair.transform.position;
        stepHeight = firstGoalStair.StepHeight;
        stepDepth = firstGoalStair.StepDepth;

        for (int i = 0; i < goalStairs.Length; i++)
        {
            if (i == 0)
            {
                goalStairs[i] = firstGoalStair;
            }
            else
            {
                goalStairs[i] = Instantiate(firstGoalStair, transform);
            }
            goalStairs[i].OnInstansiate(pos, i * stepHeight, stepColors[i % stepColors.Length]);
            pos.z += stepDepth;
            pos.y += stepHeight;
        }
    }
}
