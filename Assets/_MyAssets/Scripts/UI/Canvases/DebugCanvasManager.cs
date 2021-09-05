using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvasManager : BaseCanvasManager
{
    [Header("バナー")]
    [SerializeField] Button clearButton;
    [SerializeField] Button failButton;
    [SerializeField] Button hideBannerButton;
    [SerializeField] Button debugButton;
    [SerializeField] Image bannerImage;
    [SerializeField] Button restartButton;

    [Header("デバッグ画面")]
    [SerializeField] Image debugPanel;
    [SerializeField] Button applyButton;
    [SerializeField] Button cancelButton;

    public override void OnStart()
    {
        gameObject.SetActive(Debug.isDebugBuild);
        clearButton.onClick.AddListener(() => { Variables.screenState = ScreenState.Clear; });
        failButton.onClick.AddListener(() => { Variables.screenState = ScreenState.Failed; });
        hideBannerButton.onClick.AddListener(OnClickHideBannerButton);
        debugButton.onClick.AddListener(OnClickDebugButton);
        debugPanel.gameObject.SetActive(false);
        applyButton.onClick.AddListener(OnClickApplyButton);
        cancelButton.onClick.AddListener(OnClickCancelButton);
        restartButton.onClick.AddListener(() => base.ReLoadScene());
    }

    public override void OnInitialize()
    {
    }

    public override void OnUpdate()
    {
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
    }

    protected override void OnClose()
    {
    }

    void OnClickHideBannerButton()
    {
        bannerImage.gameObject.SetActive(!bannerImage.gameObject.activeSelf);
        hideBannerButton.GetComponent<CanvasGroup>().alpha = bannerImage.gameObject.activeSelf ? 1 : 0;
    }

    void OnClickDebugButton()
    {
        debugPanel.gameObject.SetActive(true);
    }

    void OnClickApplyButton()
    {

        Close();
    }

    void OnClickCancelButton()
    {
        Close();
    }

    void Close()
    {
        debugPanel.gameObject.SetActive(false);
    }
}
