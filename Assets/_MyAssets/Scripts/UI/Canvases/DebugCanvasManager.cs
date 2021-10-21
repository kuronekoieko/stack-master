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
    [SerializeField] InputField speedXIF;
    [SerializeField] InputField smoothTimeXIF;

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
        Time.timeScale = 0;
        speedXIF.text = Variables.speedX.ToString();
        smoothTimeXIF.text = Variables.smoothTimeX.ToString();
    }

    void OnClickApplyButton()
    {
        if (float.TryParse(speedXIF.text, out float speedX))
        {
            Variables.speedX = speedX;
        }

        if (float.TryParse(smoothTimeXIF.text, out float smoothTimeX))
        {
            Variables.smoothTimeX = smoothTimeX;
        }

        Close();
        base.ReLoadScene();
    }

    void OnClickCancelButton()
    {
        Close();
    }

    void Close()
    {
        Time.timeScale = 1f;
        debugPanel.gameObject.SetActive(false);
    }
}
