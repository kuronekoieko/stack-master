using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonDoorController : MonoBehaviour
{
    [SerializeField] Transform door_transform;
    [SerializeField] Transform doorFrame_L_transform;
    [SerializeField] Transform doorFrame_R_transform;

    [Space(10)]
    [SerializeField] float height = 5;
    [SerializeField] float openCompleteTime_sec = 0.3f;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnValidate()
    {
        door_transform.localScale = new Vector3(1, height, 1);
        doorFrame_L_transform.localScale = new Vector3(1, height + 0.1f, 1);
        doorFrame_R_transform.localScale = new Vector3(1, height + 0.1f, 1);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void OpenDoor()
    {
        door_transform.DOScaleY(0, openCompleteTime_sec).OnComplete(() => { door_transform.gameObject.SetActive(false); });
    }
}
