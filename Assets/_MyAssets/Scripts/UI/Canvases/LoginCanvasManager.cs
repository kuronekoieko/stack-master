using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityTemplate;

public class LoginCanvasManager : BaseCanvasManager
{
    [SerializeField] Button closeButton;
    [SerializeField] Button claimButton;
    [SerializeField] Button rewardVideoButton;
    [SerializeField] Image wheelImage;
    [SerializeField] Text bonusCountTextPrefab;
    [SerializeField] CoinGetAnim coinGetAnim;
    [SerializeField] int[] coinCounts;
    Text[] bonusCountTexts;

    int coinIndex;
    int coinRate;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Home);
        gameObject.SetActive(false);

        closeButton.onClick.AddListener(OnClickCloseButton);
        claimButton.onClick.AddListener(OnClickCloseButton);
        rewardVideoButton.onClick.AddListener(OnClickRewardVideoButton);

        BonusCountTextsGenerator();
        SetBonusCountTexts();
        coinGetAnim.OnStart();
    }

    public override void OnInitialize()
    {
    }

    void BonusCountTextsGenerator()
    {
        bonusCountTexts = new Text[coinCounts.Length];
        for (int i = 0; i < bonusCountTexts.Length; i++)
        {
            bonusCountTexts[i] = Instantiate(bonusCountTextPrefab, Vector3.zero, Quaternion.identity, wheelImage.rectTransform);
            float deltaDegree = -45f * i;
            float degree = 90f + deltaDegree;
            float rad = (float)(degree * Math.PI / 180);
            float radius = 425;
            Vector3 pos = Vector3.zero;
            pos.x = Mathf.Cos(rad);
            pos.y = Mathf.Sin(rad);
            pos *= radius;
            bonusCountTexts[i].rectTransform.anchoredPosition = pos;
            bonusCountTexts[i].rectTransform.eulerAngles = new Vector3(0, 0, deltaDegree + 90);
        }
    }

    public override void OnUpdate()
    {
        if (!base.IsThisScreen) { return; }
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);

        InteractableButtons(interactable: false);

        coinRate = 1;

        int length = coinCounts.Length;
        coinIndex = UnityEngine.Random.Range(0, length);
        int degree = coinIndex * 360 / length;

        RotateAnim(targetDegree: degree, OnComplete: () =>
        {
            InteractableButtons(interactable: true);
        });
    }

    void SetBonusCountTexts()
    {
        for (int i = 0; i < bonusCountTexts.Length; i++)
        {
            string numText = GetOmittedNumber(coinCounts[i]);
            bonusCountTexts[i].text = numText;
        }
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    void OnClickCloseButton()
    {
        GetCoin();
    }

    void OnClickRewardVideoButton()
    {
        // AdManager.i.RewardedVideo.ShowRewardedAd(() =>
        // {
        coinRate = 3;
        GetCoin();
        // });
    }

    void GetCoin()
    {
        InteractableButtons(interactable: false);

        coinGetAnim.Anim(() =>
        {
            //受け取り時刻の更新
            SaveData.i.receivedLoginBonusUserDateTime = Utils.DateTimeToUserDateTime(DateTime.Now);

            int coinCount = coinCounts[coinIndex] * coinRate;
            //アイテムの数を更新
            SaveData.i.coinCount += coinCount;

            SaveDataManager.i.Save();
        });
    }

    void RotateAnim(int targetDegree, Action OnComplete)
    {
        //回転数
        int n = 7;
        float sec = 8f;
        Vector3 endRotation = new Vector3(0f, 0f, -360f * n + targetDegree);
        wheelImage.rectTransform.DORotate(
            endValue: endRotation,
            duration: sec,
            mode: RotateMode.FastBeyond360)
        .SetEase(Ease.InOutQuart)
        .OnComplete(() =>
        {
            OnComplete();
        });
    }

    string GetOmittedNumber(int num)
    {
        float floatNum = num;
        int digit = num.ToString().Length;
        if (digit < 6) { return num.ToString(); }

        string text = (floatNum / 1000).ToString().Substring(0, 3);
        if (digit == 6) { return text + "K"; }

        text = (floatNum / 1000000).ToString().Substring(0, 3);
        if (digit == 9) { return text + "M"; }
        text = (floatNum / 1000000).ToString().Substring(0, 4);
        if (digit < 10) { return text + "M"; }
        text = (floatNum / 1000000000).ToString().Substring(0, 4);
        return text + "B";
    }

    void InteractableButtons(bool interactable)
    {
        closeButton.interactable = interactable;
        claimButton.interactable = interactable;
        rewardVideoButton.interactable = interactable;
    }

}
