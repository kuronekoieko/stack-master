using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        selectedImage.gameObject.SetActive(isSelected);
        arrowImage.gameObject.SetActive(isSelected);
        levelText.text = level.ToString();
        clearedImage.gameObject.SetActive(isCleared);
        // transform.localScale = isSelected ? Vector3.one * 1.3f : Vector3.one;
    }
}
