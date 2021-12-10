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
    GameObject hatObject;
    public SkinSelectState SelectState { get; set; }
    int skinIndex;

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
        hatObject = Instantiate(SkinSettingSO.i.characterSkinDatas[skinIndex].prefab, Vector3.zero, Quaternion.identity, transform);
        hatObject.layer = LayerMask.NameToLayer("UI");
        hatObject.transform.localPosition = Vector3.forward * -100;
        hatObject.transform.localScale = Vector3.one * 20;
        hatObject.transform.localPosition -= Vector3.up * 50f;
        hatObject.transform.up = new Vector3(1, 5, 0);
        hatObject.gameObject.SetActive(false);
        image.sprite = lockSprite;
        SelectState = SkinSelectState.Lock;
    }

    void ChangeView(SkinSelectState selectState)
    {
        switch (selectState)
        {
            case SkinSelectState.Lock:
                hatObject.SetActive(false);
                image.sprite = lockSprite;
                selectbutton.enabled = false;
                break;
            case SkinSelectState.Unlock:
                hatObject.SetActive(true);
                image.sprite = unlockSprite;
                selectbutton.enabled = true;
                break;
            case SkinSelectState.Select:
                image.sprite = selectedSprite;
                hatObject.SetActive(true);
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
