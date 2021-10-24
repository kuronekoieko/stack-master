using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] Color fogColor;
    public BackgroundManager BackgroundManager { get; set; }

    void OnValidate()
    {
        var cameraController = FindObjectOfType<CameraController>();
        if (cameraController == null) return;
        cameraController.GetComponent<Camera>().backgroundColor = fogColor;
        RenderSettings.fogColor = fogColor;
    }


    public void Activate(Camera cam)
    {
        cam.backgroundColor = fogColor;
        RenderSettings.fogColor = fogColor;
        gameObject.SetActive(true);
    }
}
