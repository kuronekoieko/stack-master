using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SkinCanvasManager : BaseCanvasManager
{
    [SerializeField] Button closeButton_arrow;
    [SerializeField] Button closeButton_x;
    [SerializeField] TabController_Skin tabController_Skin;
    [SerializeField] TabController_Material tabController_Material;
    [SerializeField] Transform skinPreviewParent;
    SkinController previewSkin;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Skin);
        gameObject.SetActive(false);
        closeButton_arrow.onClick.AddListener(OnClickCloseButton);
        closeButton_x.onClick.AddListener(OnClickCloseButton);

        this.ObserveEveryValueChanged(_ => SaveData.i.selectedSkinIndex)
            .Subscribe(_ => OnChangedSelected(_));

        tabController_Skin.OnStart();
        tabController_Material.OnStart();
    }

    void OnTabChanged(bool isOn)
    {

    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
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


    void OnChangedSelected(int selectedButtonIndex)
    {
        if (previewSkin) DestroyImmediate(previewSkin.gameObject);
        previewSkin = Instantiate(SkinSettingSO.i.characterSkinDatas[selectedButtonIndex].prefab, skinPreviewParent);
        previewSkin.OnInstantiate();
        previewSkin.ChangeLayersForAllChildren("Skin");
    }

}
