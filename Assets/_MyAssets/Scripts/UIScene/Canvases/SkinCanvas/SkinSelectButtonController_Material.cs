using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkinSelectButtonController_Material : MonoBehaviour, ISkinSelectButtonController
{
    bool IsOwn => SaveData.i.materialSkinSaveDatas[skinIndex].isOwn;
    bool IsSelected => SaveData.i.selectedMaterialIndex == skinIndex;
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

        MeshRenderer sphereMr = Instantiate(skinSelectButtonController.sphereMrPrefab, skinSelectButtonController.skinPreviewParent);
        skinSelectButtonController.skinObj = sphereMr.gameObject;
        skinSelectButtonController.skinObj.layer = LayerMask.NameToLayer("Skin");
        sphereMr.material = new Material(SkinSettingSO.i.characterMaterialDatas[skinIndex].material);
        skinSelectButtonController.skinObj.SetActive(false);
    }

    void OnClickSelectButton()
    {
        if (skinSelectButtonController.SelectState != SkinSelectState.Unlock) { return; }
        SaveData.i.selectedMaterialIndex = skinIndex;
        SaveDataManager.i.Save();
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
