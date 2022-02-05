using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalController : MonoBehaviour
{
    [SerializeField] GoalStairController goalStairPrefab;
    [SerializeField] Transform firstGoalStairTf;
    [SerializeField] Color[] stepColors;
    GoalStairController[] goalStairs;
    public static GoalController i;


    void Awake()
    {
        i = this;
        int stepCount = 51;
        goalStairs = new GoalStairController[stepCount];
        Vector3 pos = firstGoalStairTf.position;
        float stepHeight = goalStairPrefab.StepHeight;
        float stepDepth = goalStairPrefab.StepDepth;

        for (int i = 0; i < goalStairs.Length; i++)
        {
            goalStairs[i] = Instantiate(goalStairPrefab, transform);
            float rate = 1.1f + (float)i * 0.1f;
            goalStairs[i].OnInstansiate(
                isLast: i == goalStairs.Length - 1,
                rate: rate,
                pos: pos,
                height: i * stepHeight,
                stepColor: stepColors[i % stepColors.Length]);
            pos.z += stepDepth;
            pos.y += stepHeight;
        }
    }
}
