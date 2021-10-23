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


    void Start()
    {
        tutrialHandController.DragHorizontalAnim(leftPosRt.anchoredPosition3D, rightPosRt.anchoredPosition3D);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tutrialHandController.Kill();
            gameObject.SetActive(false);
        }
    }
}
