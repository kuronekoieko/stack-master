using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MyButton : Button
{
    Text text;
    public Text Text
    {
        get
        {
            if (text == null) text = GetComponentInChildren<Text>();
            return text;
        }
    }

    Tween tween;

    protected override void OnDisable()
    {
        base.OnDisable();
        // 親が非表示になったときも呼ばれる
        if (tween != null) tween.Kill();
    }

    public void Show_ScaleAnim()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;

        transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack)
        .OnComplete(() =>
        {
            tween = transform.DOScale(Vector3.one * 1.1f, 0.8f).SetEase(Ease.Flash, 2).SetLoops(-1);
        });
    }

    public void Show_FadeAnim(float delay = 0)
    {
        tween = DOVirtual.DelayedCall(delay, () =>
        {
            gameObject.SetActive(true);
            Color color = image.color;
            color.a = 0;
            image.color = color;
            image.DOFade(1f, 1.5f);
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


}
