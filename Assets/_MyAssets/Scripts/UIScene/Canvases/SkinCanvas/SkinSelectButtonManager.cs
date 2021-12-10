using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnapScroll;
using DG.Tweening;
using UniRx;
using UnityEngine.UI;

public class SkinSelectButtonManager : MonoBehaviour
{
    [SerializeField] SkinSelectButtonController skinSelectPrefab;
    [SerializeField] RectTransform scrollViewContent;
    [SerializeField] public SnapScrollView scrollView;
    [SerializeField] Image originIndicator;
    [SerializeField] Transform indicatorParent;
    [SerializeField] Sprite activeIndicatorSprite;
    [SerializeField] Sprite inActiveIndicatorSprite;
    Image[] indicators;
    SkinSelectButtonController[] skinSelectControllers = new SkinSelectButtonController[0];//初期化時nullのため

    public void OnStart()
    {
        Generator();
        this.ObserveEveryValueChanged(selectedSkinIndex => SaveData.i.selectedSkinIndex)
            .Subscribe(selectedSkinIndex => OnChangedSkin(selectedSkinIndex));
    }

    void Generator()
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
        GenerateIndicators();
        scrollView.RefreshPage();
    }


    void GenerateIndicators()
    {
        indicators = new Image[scrollView.MaxPage + 1];

        float offset = 60f;
        Vector3 pos = -Vector3.right * offset * (indicators.Length - 1) / 2f;
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i] = Instantiate(originIndicator, indicatorParent);
            indicators[i].rectTransform.anchoredPosition3D = pos;
            pos.x += offset;
        }
        originIndicator.gameObject.SetActive(false);
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

    public void UpdateAllButtonsView(int selectedSkinIndex)
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

}
