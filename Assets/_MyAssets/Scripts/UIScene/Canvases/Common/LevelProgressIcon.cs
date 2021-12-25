using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelProgressIcon : MonoBehaviour
{
    [SerializeField] Image notSelectedImage;
    [SerializeField] Image selectedImage;
    [SerializeField] Image arrowImage;
    [SerializeField] Text levelText;
    [SerializeField] Image clearedImage;

    public void Show(int level, bool isSelected, bool isCleared)
    {
        notSelectedImage.gameObject.SetActive(!isSelected);
        selectedImage.gameObject.SetActive(isSelected || isCleared);
        arrowImage.gameObject.SetActive(isSelected);
        levelText.text = level.ToString();
        clearedImage.gameObject.SetActive(isCleared);
        // transform.localScale = isSelected ? Vector3.one * 1.3f : Vector3.one;
    }

    public void Anim()
    {
        clearedImage.gameObject.SetActive(true);
        clearedImage.fillAmount = 0;
        DOTween.To
        (
            () => clearedImage.fillAmount,       //何に
            (x) => clearedImage.fillAmount = x,  //何を
            1f,     //どこまで(最終的な値)
            1f		//どれくらいの時間
        );
    }
}
