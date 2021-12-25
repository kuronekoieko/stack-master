using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class AddCountTextEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    RectTransform rectTransform;
    public Transform target;


    public void OnInstantiate()
    {
        gameObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
    }



    public void Show(int count)
    {
        gameObject.SetActive(true);
        textMeshProUGUI.text = "+" + count;
        textMeshProUGUI.transform.localPosition = Vector3.zero;
        textMeshProUGUI.transform.DOLocalMoveY(200f * (float)Screen.height / 1334f, 1.0f).SetRelative();
        textMeshProUGUI.SetAlpha(1f);
        textMeshProUGUI.DOFade(0, 0.7f).SetDelay(0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(AddCountTextEffectManager.i.uiCamera, target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(AddCountTextEffectManager.i.canvasRt, screenPos, AddCountTextEffectManager.i.uiCamera, out Vector2 pos);
        Vector2 offset;
        offset.x = 150f * (float)Screen.width / 1334f;
        offset.y = 0f * (float)Screen.height / 1334f;
        rectTransform.anchoredPosition3D = pos + offset;
    }
}
