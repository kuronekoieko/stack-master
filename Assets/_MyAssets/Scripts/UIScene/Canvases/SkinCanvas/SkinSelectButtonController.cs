using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Events;

public enum SkinSelectState
{
    Lock = 0,
    Unlock = 1,
    Select = 2,
    Dummy = 3,
}

public class SkinSelectButtonController : MonoBehaviour
{
    [SerializeField] MyButton selectbutton;
    [SerializeField] Sprite unlockSprite;
    [SerializeField] Sprite lockSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Image image;
    [SerializeField] RectTransform rectTransform;
    public Transform skinPreviewParent;
    public MeshRenderer sphereMrPrefab;

    public GameObject skinObj { get; set; }
    public SkinSelectState SelectState { get; set; }
    public int skinIndex { get; private set; }
    public UnityAction OnClickSelectButton { set => selectbutton.onClick.AddListener(value); }

    public void OnInstantiate(int skinIndex, bool isDummy)
    {
        this.skinIndex = skinIndex;

        // Vector3 pos = rectTransform.anchoredPosition3D;
        //  pos.z = 0;
        // rectTransform.anchoredPosition3D = pos;
        rectTransform.SetAnchoredPosition3DZ(0);
        if (isDummy)
        {
            SelectState = SkinSelectState.Dummy;
            image.enabled = false;
            selectbutton.enabled = false;
            return;
        }

        image.sprite = lockSprite;
        SelectState = SkinSelectState.Lock;



        this.ObserveEveryValueChanged(_ => SelectState)
            .Subscribe(_ => ChangeView(_));
    }

    void ChangeView(SkinSelectState selectState)
    {
        switch (selectState)
        {
            case SkinSelectState.Lock:
                skinObj?.gameObject.SetActive(false);
                image.sprite = lockSprite;
                selectbutton.enabled = false;
                break;
            case SkinSelectState.Unlock:
                skinObj?.gameObject.SetActive(true);
                image.sprite = unlockSprite;
                selectbutton.enabled = true;
                break;
            case SkinSelectState.Select:
                image.sprite = selectedSprite;
                skinObj?.gameObject.SetActive(true);
                selectbutton.enabled = false;
                break;
            case SkinSelectState.Dummy:
                break;
            default:
                break;
        }
    }

}
