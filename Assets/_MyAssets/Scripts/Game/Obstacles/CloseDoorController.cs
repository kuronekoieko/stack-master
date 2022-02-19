using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloseDoorController : MonoBehaviour
{
    [SerializeField] Transform door_L_transform;
    [SerializeField] Transform door_R_transform;

    [Space(10)]
    [SerializeField] float height = 5;
    [SerializeField] float closeCompleteTime_sec = 0.3f;

    List<CloseButtonController> buttons = new List<CloseButtonController>();
    bool wasClosed = false;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out CloseButtonController button))
            {
                buttons.Add(button);
            }
        }
    }

    void OnValidate()
    {
        door_L_transform.localScale = new Vector3(1, height, 1);
        door_R_transform.localScale = new Vector3(1, height, 1);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void CloseDoor()
    {
        door_L_transform.DORotate(new Vector3(0, 0, 0), closeCompleteTime_sec);
        door_R_transform.DORotate(new Vector3(0, 0, 0), closeCompleteTime_sec);
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].DisableButton();
        }
    }
}
