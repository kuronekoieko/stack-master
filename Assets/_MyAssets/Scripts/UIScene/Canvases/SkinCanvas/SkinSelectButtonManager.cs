using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnapScroll;
using DG.Tweening;
using UniRx;
using UnityEngine.UI;
using System.Linq;

public class SkinSelectButtonManager : MonoBehaviour
{
    [SerializeField] SkinSelectButtonController skinSelectPrefab;
    [SerializeField] RectTransform scrollViewContent;
    [SerializeField] public SnapScrollView scrollView;
    [SerializeField] Image originIndicator;
    [SerializeField] Transform indicatorParent;
    [SerializeField] Sprite activeIndicatorSprite;
    [SerializeField] Sprite inActiveIndicatorSprite;
    [SerializeField] Button scrollButton_Right;
    [SerializeField] Button scrollButton_Left;
    Image[] indicators;
    SkinSelectButtonController[] skinSelectControllers = new SkinSelectButtonController[0];//初期化時nullのため
    int contentsCountPerPage = 9;
    int gemCountForUnlockRandom = 1;
    public bool EnableUnlockRandom => SaveData.i.currencyCount >= gemCountForUnlockRandom && notOwnIndexes.Count > 0;
    List<int> notOwnIndexes = new List<int>();

    public void OnStart()
    {
        Generator();
        this.ObserveEveryValueChanged(_ => SaveData.i.selectedSkinIndex)
            .Subscribe(_ => OnChangedSkin(_));
        scrollButton_Right.onClick.AddListener(() => OnClickScrollButton(true));
        scrollButton_Left.onClick.AddListener(() => OnClickScrollButton(false));
    }

    void Generator()
    {
        if (skinSelectControllers.Length > 0) { return; }


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
        scrollView.OnPageChanged += OnPageChanged;
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

    void OnPageChanged()
    {
        for (var i = 0; i < indicators.Length; i++)
        {
            indicators[i].sprite = (i == scrollView.Page) ? activeIndicatorSprite : inActiveIndicatorSprite;
        }

        scrollButton_Left.gameObject.SetActive(scrollView.Page != 0);
        scrollButton_Right.gameObject.SetActive(scrollView.Page != scrollView.MaxPage);

        SetNotOwnIndexes();
    }


    void OnChangedSkin(int selectedSkinIndex)
    {
        UpdateAllButtonsView(selectedSkinIndex);
    }

    public void OnOpen()
    {
        UpdateAllButtonsView(SaveData.i.selectedSkinIndex);
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

            if (i != selectedSkinIndex)
            {
                skinSelectControllers[i].SelectState = SkinSelectState.Unlock;
                continue;
            }

            skinSelectControllers[i].SelectState = SkinSelectState.Select;
        }
    }

    void OnClickScrollButton(bool isRight)
    {
        int page = scrollView.Page;
        if (isRight)
        {
            page++;
        }
        else
        {
            page--;
        }
        scrollView.Page = Mathf.Clamp(page, 0, scrollView.MaxPage);
        scrollView.RefreshPage();
    }

    public void UnlockRandom()
    {
        SetNotOwnIndexes();
        int randomInt = notOwnIndexes[Random.Range(0, notOwnIndexes.Count)];
        SaveData.i.currencyCount -= gemCountForUnlockRandom;
        SaveData.i.characterSkinSaveDatas[randomInt].isOwn = true;
        SaveData.i.selectedSkinIndex = randomInt;
        SaveDataManager.i.Save();
        SetNotOwnIndexes();
    }


    void SetNotOwnIndexes()
    {
        int upperLeftIndex = scrollView.Page * contentsCountPerPage;
        int nextPageUpperLeftIndex = Mathf.Clamp(upperLeftIndex + contentsCountPerPage, 0, skinSelectControllers.Length);
        notOwnIndexes.Clear();
        for (int i = upperLeftIndex; i < nextPageUpperLeftIndex; i++)
        {
            if (SaveData.i.characterSkinSaveDatas[i].isOwn) continue;
            notOwnIndexes.Add(i);
        }
    }
}
