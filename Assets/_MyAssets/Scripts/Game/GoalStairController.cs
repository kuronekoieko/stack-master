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
    [SerializeField] ParticleSystem confettiDirectional_L;
    [SerializeField] ParticleSystem confettiDirectional_R;

    public float StepHeight => stepMr.transform.localScale.y;
    public float StepDepth => stepMr.transform.localScale.z;
    float goalRate;
    public bool isLast { get; private set; }

    public void OnInstansiate(bool isLast, float rate, Vector3 pos, float height, Color stepColor)
    {
        this.isLast = isLast;
        transform.position = pos;
        stepMr.material.color = stepColor;
        stepBaseMr.transform.localPosition -= Vector3.up * (height + StepHeight / 2f) / 2f;
        Vector3 scale = stepBaseMr.transform.localScale;
        scale.y = height;
        stepBaseMr.transform.localScale = scale;
        textMeshPro.text = "x " + rate.ToString("F1");
        goalRate = rate;

        if (isLast)
        {
            stepMr.material.color = Color.gray;
            textMeshPro.gameObject.SetActive(false);
        }
    }

    public void Selected()
    {
        Variables.goalRate = goalRate;
        stepMr.material.DOColor(Color.white, 1f).SetEase(Ease.InOutFlash, 2).SetLoops(-1);
        textMeshPro.DOFade(0, 1f).SetEase(Ease.InOutFlash, 2).SetLoops(-1);
        confetti_L.Play();
        confetti_R.Play();
    }

    public void Passed()
    {
        confettiDirectional_L.Play();
        confettiDirectional_R.Play();
    }
}
