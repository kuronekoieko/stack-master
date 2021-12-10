using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SnapScroll;
using DG.Tweening;
using UniRx;

public class SkinCanvasManager : BaseCanvasManager
{
    [SerializeField] Button closeButton_arrow;
    [SerializeField] Button closeButton_x;
    [SerializeField] Button unlockButton;
    [SerializeField] Button rewardedButton;
    [SerializeField] SkinSelectButtonController skinSelectPrefab;
    [SerializeField] RectTransform scrollViewContent;
    [SerializeField] public SnapScrollView scrollView;
    [SerializeField] Image[] indicators;
    [SerializeField] Sprite activeIndicatorSprite;
    [SerializeField] Sprite inActiveIndicatorSprite;
    SkinSelectButtonController[] skinSelectControllers = new SkinSelectButtonController[0];//初期化時nullのため


    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Skin);
        gameObject.SetActive(false);
        closeButton_arrow.onClick.AddListener(OnClickCloseButton);
        closeButton_x.onClick.AddListener(OnClickCloseButton);
        unlockButton.onClick.AddListener(OnClickUnlockButton);
        rewardedButton.onClick.AddListener(OnClickRewardedButton);

        Generator();
        this.ObserveEveryValueChanged(selectedSkinIndex => SaveData.i.selectedSkinIndex)
        .Subscribe(selectedSkinIndex => OnChangedSkin(selectedSkinIndex));
    }

    public void Generator()
    {
        if (skinSelectControllers.Length > 0) { return; }
        int contentsCountPerPage = 9;

        skinSelectControllers = new SkinSelectButtonController[SkinSettingSO.i.characterSkinDatas.Length];
        for (int i = 0; i < skinSelectControllers.Length; i++)
        {
            skinSelectControllers[i] = Instantiate(skinSelectPrefab, scrollViewContent);
            skinSelectControllers[i].OnInstantiate(i, false);
        }

        int dummyButtonCount = skinSelectControllers.Length - skinSelectControllers.Length % contentsCountPerPage;
        for (int i = 0; i < dummyButtonCount; i++)
        {
            Instantiate(skinSelectPrefab, scrollViewContent).OnInstantiate(i, true);
        }

        /// 【UnityAsset】SnapScroll – iPhoneのホーム画面のようなスナップスクロールを作る
        /// https://tempura-kingdom.jp/snapscroll/

        scrollView.MaxPage = Mathf.CeilToInt((float)skinSelectControllers.Length / (float)contentsCountPerPage) - 1;
        // scrollView.PageSize = scrollView.GetComponent<RectTransform>().sizeDelta.x;
        scrollView.PageSize = 800;
        scrollView.ScrollableDistance = 0.01f;//スワイプ感度
        scrollView.OnPageChanged += OnIndicatorUpdate;
        scrollView.RefreshPage();
    }

    void OnIndicatorUpdate()
    {
        for (var i = 0; i < indicators.Length; i++)
        {
            indicators[i].sprite = (i == scrollView.Page) ? activeIndicatorSprite : inActiveIndicatorSprite;
        }
    }

    void OnChangedSkin(int selectedSkinIndex)
    {
        UpdateAllButtonsView(selectedSkinIndex);
    }

    void UpdateAllButtonsView(int selectedSkinIndex)
    {
        for (int i = 0; i < skinSelectControllers.Length; i++)
        {
            if (!SaveData.i.characterSkinSaveDatas[i].isOwn)
            {
                skinSelectControllers[i].SelectState = SkinSelectState.Lock;
                continue;
            }

            skinSelectControllers[i].SelectState = (i == selectedSkinIndex) ? SkinSelectState.Select : SkinSelectState.Unlock;
        }
    }


    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        UpdateAllButtonsView(SaveData.i.selectedSkinIndex);
    }

    public override void OnUpdate()
    {

    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    public override void OnSceneLoaded()
    {

    }

    void OnClickCloseButton()
    {
        //StageTransManager.i.ReLoadStage();
        Variables.screenState = ScreenState.Start;
    }

    void OnClickUnlockButton()
    {

    }

    void OnClickRewardedButton()
    {

    }
}
