using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public enum SkinSelectState
{
    Lock = 0,
    Unlock = 1,
    Select = 2,
    Dummy = 3,
}

public class SkinSelectButtonController : MonoBehaviour
{
    [SerializeField] Button selectbutton;
    [SerializeField] Sprite unlockSprite;
    [SerializeField] Sprite lockSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Image image;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Transform skinPreviewParent;
    SkinController skinController;
    public SkinSelectState SelectState { get; set; }
    int skinIndex;



    private void Awake()
    {
        this.ObserveEveryValueChanged(selectState => SelectState)
        .Subscribe(selectState => ChangeView(selectState));
        selectbutton.onClick.AddListener(OnClickSelectButton);
    }

    public void OnInstantiate(int skinIndex, bool isDummy)
    {
        this.skinIndex = skinIndex;
        Vector3 pos = rectTransform.anchoredPosition3D;
        pos.z = 0;
        rectTransform.anchoredPosition3D = pos;
        if (isDummy)
        {
            SelectState = SkinSelectState.Dummy;
            return;
        }
        skinController = Instantiate(SkinSettingSO.i.characterSkinDatas[skinIndex].prefab, skinPreviewParent);
        skinController.OnInstantiate(SkinSettingSO.i.characterMaterialDatas[0].material);
        skinController.ChangeLayers(skinController.transform, "Skin");
        skinController.gameObject.SetActive(false);
        image.sprite = lockSprite;
        SelectState = SkinSelectState.Lock;
    }




    void ChangeView(SkinSelectState selectState)
    {
        switch (selectState)
        {
            case SkinSelectState.Lock:
                skinController.gameObject.SetActive(false);
                image.sprite = lockSprite;
                selectbutton.enabled = false;
                break;
            case SkinSelectState.Unlock:
                skinController.gameObject.SetActive(true);
                image.sprite = unlockSprite;
                selectbutton.enabled = true;
                break;
            case SkinSelectState.Select:
                image.sprite = selectedSprite;
                skinController.gameObject.SetActive(true);
                selectbutton.enabled = false;
                break;
            case SkinSelectState.Dummy:
                image.enabled = false;
                selectbutton.enabled = false;
                break;
            default:
                break;
        }
    }

    void OnClickSelectButton()
    {
        if (SelectState != SkinSelectState.Unlock) { return; }
        SaveData.i.selectedSkinIndex = skinIndex;
        SaveDataManager.i.Save();
        // ここではセーブデータの入れ替えだけにして、実際の処理はunirxで起動するようにする
    }
}
