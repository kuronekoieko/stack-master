using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoticeImageController : MonoBehaviour
{

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOPunchScale(new Vector2(-0.3f, 0.3f), 3, 5, 10).SetLoops(-1).SetEase(Ease.InQuint);//プルプルさせる（同じ方向に震える）（方向、時間、振動、弾性）
    }


}
