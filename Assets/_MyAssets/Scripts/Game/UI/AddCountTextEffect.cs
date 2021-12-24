using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class AddCountTextEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] Camera uiCamera;
    [SerializeField] RectTransform canvasRt;
    RectTransform rectTransform;
    public static AddCountTextEffect i;

    void Awake()
    {
        i = this;
        gameObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
    }



    public void Show(int count, Vector3 worldPos)
    {
        gameObject.SetActive(true);
        textMeshProUGUI.text = "+" + count;
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRt, screenPos, uiCamera, out Vector2 pos);
        Vector2 offset;
        offset.x = 0f * (float)Screen.width / 1334f;
        offset.y = 150f * (float)Screen.height / 1334f;
        rectTransform.anchoredPosition3D = pos + offset;
        rectTransform.DOLocalMoveY(100f * (float)Screen.height / 1334f, 1.0f).SetRelative();
        textMeshProUGUI.SetAlpha(1f);
        textMeshProUGUI.DOFade(0, 0.7f).SetDelay(0.3f);
    }
}
