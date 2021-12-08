using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class SwitchSEButtonView : MonoBehaviour
{
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    Button switchSEButton;
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        switchSEButton = GetComponent<Button>();
    }

    void Start()
    {
        switchSEButton.onClick.AddListener(OnClickSwitchSEButton);

        //特定の画面で変わったとき、他の画面にも反映させるため
        this.ObserveEveryValueChanged(isOffSE => SaveData.i.isOffSE)
            .Subscribe(isOffSE => { SetSprite(); })
            .AddTo(this.gameObject);
    }

    void OnClickSwitchSEButton()
    {
        SaveData.i.isOffSE = !SaveData.i.isOffSE;
        SaveDataManager.i.Save();
    }

    void SetSprite()
    {
        image.sprite = SaveData.i.isOffSE ? offSprite : onSprite;
    }


}
