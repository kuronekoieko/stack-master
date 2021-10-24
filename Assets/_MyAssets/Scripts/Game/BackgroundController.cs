using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] Color fogColor;

    void OnValidate()
    {
        Camera.main.backgroundColor = fogColor;
        RenderSettings.fogColor = fogColor;
    }


    public void Activate()
    {
        Camera.main.backgroundColor = fogColor;
        RenderSettings.fogColor = fogColor;
        gameObject.SetActive(true);
    }
}
