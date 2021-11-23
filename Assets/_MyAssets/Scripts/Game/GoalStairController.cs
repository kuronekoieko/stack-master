using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GoalStairController : MonoBehaviour
{

    [SerializeField] MeshRenderer stepMr;
    [SerializeField] MeshRenderer stepBaseMr;
    [SerializeField] TextMeshPro textMeshPro;
    [SerializeField] ParticleSystem confetti_L;
    [SerializeField] ParticleSystem confetti_R;

    public float StepHeight => stepMr.transform.localScale.y;
    public float StepDepth => stepMr.transform.localScale.z;
    float goalRate;

    public void OnInstansiate(float rate, Vector3 pos, float height, Color stepColor)
    {
        transform.position = pos;
        stepMr.material.color = stepColor;
        stepBaseMr.transform.localPosition -= Vector3.up * (height + StepHeight / 2f) / 2f;
        Vector3 scale = stepBaseMr.transform.localScale;
        scale.y = height;
        stepBaseMr.transform.localScale = scale;
        textMeshPro.text = "x " + rate.ToString("F1");
        goalRate = rate;
    }

    public void Selected()
    {
        Variables.goalRate = goalRate;
        stepMr.material.DOColor(Color.white, 1f).SetEase(Ease.InOutFlash, 2).SetLoops(-1);
        confetti_L.Play();
        confetti_R.Play();
    }
}
