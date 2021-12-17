using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvasManager : BaseCanvasManager
{
    [Header("バナー")]
    [SerializeField] MyButton clearButton;
    [SerializeField] MyButton failButton;
    [SerializeField] MyButton hideBannerButton;
    [SerializeField] MyButton debugButton;
    [SerializeField] Image bannerImage;
    [SerializeField] MyButton restartButton;

    [Header("デバッグ画面")]
    [SerializeField] Image debugPanel;
    [SerializeField] MyButton applyButton;
    [SerializeField] MyButton cancelButton;
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
        restartButton.onClick.AddListener(() => StageTransManager.i.ReLoadStage());
    }

    public override void OnSceneLoaded()
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
        StageTransManager.i.ReLoadStage();
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
