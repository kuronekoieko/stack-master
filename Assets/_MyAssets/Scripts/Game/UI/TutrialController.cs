using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutrialController : MonoBehaviour
{
    [SerializeField] Image arrowImage;
    [SerializeField] TutrialHandController tutrialHandController;
    [SerializeField] RectTransform leftPosRt;
    [SerializeField] RectTransform rightPosRt;


    public void OnOpen()
    {
        gameObject.SetActive(true);
        tutrialHandController.DragHorizontalAnim(leftPosRt.anchoredPosition3D, rightPosRt.anchoredPosition3D);
    }

    public void OnClose()
    {
        tutrialHandController.Kill();
    }
}
