using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class CoinCountView : MonoBehaviour
{
    [SerializeField] Text coinCountText;
    int coinCount;
    public void OnStart()
    {
        this.ObserveEveryValueChanged(coinCount => SaveData.i.coinCount)
            .Subscribe(coinCount => CountUpAnim())
            .AddTo(this.gameObject);

        this.ObserveEveryValueChanged(coinCount => this.coinCount)
            .Subscribe(coinCount => coinCountText.text = coinCount.ToString())
            .AddTo(this.gameObject);
    }

    void CountUpAnim()
    {
        DOTween.To(() => coinCount, (x) => coinCount = x, SaveData.i.coinCount, 0.5f);
    }


}
