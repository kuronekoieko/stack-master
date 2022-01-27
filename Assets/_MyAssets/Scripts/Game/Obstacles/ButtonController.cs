using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonController : MonoBehaviour
{
    [SerializeField] ButtonDoorController buttonDoorController;
    [SerializeField] Transform buttonTrigger_transform;
    [SerializeField] Renderer buttonTrigger_renderer;

    [Space(5)]
    [SerializeField] Transform cable_vertical_transform;
    [SerializeField] Transform cable_horizon_transform;
    [SerializeField] Transform cable_sphere_transform;

    [Space(10)]
    [SerializeField] Vector2 buttonPosition;
    [SerializeField] float cableWidth = 1;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnValidate()
    {
        transform.localPosition = new Vector3(buttonPosition.x, 0, buttonPosition.y);

        float posX = 3.9f;
        if (buttonPosition.x < 0) posX *= -1;
        Vector3 pos = transform.localPosition;
        pos.x = posX;
        pos.y = 0.25f;
        cable_sphere_transform.localPosition = pos;
        cable_horizon_transform.localPosition = pos;
        cable_vertical_transform.localPosition = pos;
        float length_h = (transform.position.x - posX) / 2;
        cable_horizon_transform.localScale = new Vector3(length_h, cableWidth, cableWidth);
        cable_vertical_transform.localScale = new Vector3(cableWidth, cableWidth, -buttonPosition.y / 2);
        cable_sphere_transform.localScale = new Vector3(cableWidth / 2, cableWidth / 2, cableWidth / 2);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerEnter(Collider collider)
    {
        OnTouchPlayer(collider);
    }

    void OnTouchPlayer(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;

        buttonTrigger_renderer.material.color = Color.black;
        buttonTrigger_transform.DOLocalMoveY(-0.15f, 0.2f);
        buttonDoorController.OpenDoor();
    }
}
