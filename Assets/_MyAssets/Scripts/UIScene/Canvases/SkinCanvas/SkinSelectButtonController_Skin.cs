using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkinSelectButtonController_Skin : MonoBehaviour, ISkinSelectButtonController
{
    bool IsOwn => SaveData.i.characterSkinSaveDatas[skinIndex].isOwn;
    bool IsSelected => SaveData.i.selectedSkinIndex == skinIndex;
    int skinIndex;
    SkinSelectButtonController skinSelectButtonController;

    public void OnInstantiate()
    {
        skinSelectButtonController = GetComponent<SkinSelectButtonController>();
        this.skinIndex = skinSelectButtonController.skinIndex;

        this.ObserveEveryValueChanged(_ => IsSelected)
            .Subscribe(_ => OnChangedSaveData());

        this.ObserveEveryValueChanged(_ => IsOwn)
            .Subscribe(_ => OnChangedSaveData());

        skinSelectButtonController.OnClickSelectButton = OnClickSelectButton;


        SkinController skinController;
        skinController = Instantiate(SkinSettingSO.i.characterSkinDatas[skinIndex].prefab, skinSelectButtonController.skinPreviewParent);
        skinController.OnInstantiate();
        skinController.ChangeLayersForAllChildren("Skin");
        skinSelectButtonController.skinObj = skinController.gameObject;
        skinSelectButtonController.skinObj.SetActive(false);
    }

    void OnClickSelectButton()
    {
        if (skinSelectButtonController.SelectState != SkinSelectState.Unlock) { return; }
        SaveData.i.selectedSkinIndex = skinIndex;
        SaveDataManager.i.Save();
        SoundManager.i?.PlayOneShot(3);
        // ここではセーブデータの入れ替えだけにして、実際の処理はunirxで起動するようにする
    }

    public void OnChangedSaveData()
    {
        if (!IsOwn)
        {
            skinSelectButtonController.SelectState = SkinSelectState.Lock;
            return;
        }

        if (!IsSelected)
        {
            skinSelectButtonController.SelectState = SkinSelectState.Unlock;
            return;
        }
        skinSelectButtonController.SelectState = SkinSelectState.Select;
    }
}
