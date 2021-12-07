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

    public void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tutrialHandController.Kill();
            gameObject.SetActive(false);
            Variables.screenState = ScreenState.Game;
        }
    }


}
