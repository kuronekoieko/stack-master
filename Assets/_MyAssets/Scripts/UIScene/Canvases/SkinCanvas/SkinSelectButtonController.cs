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
    [SerializeField] Material characterMaterial;
    GameObject skinObject;
    public SkinSelectState SelectState { get; set; }
    int skinIndex;
    SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        this.ObserveEveryValueChanged(selectState => SelectState)
        .Subscribe(selectState => ChangeView(selectState))
        .AddTo(this.gameObject);
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
        skinObject = Instantiate(SkinSettingSO.i.characterSkinDatas[skinIndex].prefab, Vector3.zero, Quaternion.identity, transform);
        ChangeLayerChildren(skinObject.transform, "UI");
        skinObject.transform.localPosition = new Vector3(0, -80f, -100f);
        skinObject.transform.localScale = Vector3.one * 95f;
        skinObject.transform.eulerAngles = Vector3.up * -158f;
        skinnedMeshRenderer = skinObject.GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMeshRenderer.material = characterMaterial;
        skinObject.gameObject.SetActive(false);
        image.sprite = lockSprite;
        SelectState = SkinSelectState.Lock;
    }

    void ChangeLayerChildren(Transform transform, string layerName)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
            ChangeLayerChildren(child, layerName);
        }
    }


    void ChangeView(SkinSelectState selectState)
    {
        switch (selectState)
        {
            case SkinSelectState.Lock:
                skinObject.SetActive(false);
                image.sprite = lockSprite;
                selectbutton.enabled = false;
                break;
            case SkinSelectState.Unlock:
                skinObject.SetActive(true);
                image.sprite = unlockSprite;
                selectbutton.enabled = true;
                break;
            case SkinSelectState.Select:
                image.sprite = selectedSprite;
                skinObject.SetActive(true);
                selectbutton.enabled = true;
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
        if (SelectState == SkinSelectState.Lock) { return; }
        if (SelectState == SkinSelectState.Select)
        {
            SaveData.i.selectedSkinIndex = -1;
            SaveDataManager.i.Save();
            return;
        }
        SaveData.i.selectedSkinIndex = skinIndex;
        SaveDataManager.i.Save();
        // ここではセーブデータの入れ替えだけにして、実際の処理はunirxで起動するようにする
    }
}
