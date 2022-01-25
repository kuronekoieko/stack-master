using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HightGateController : MonoBehaviour
{
    [SerializeField] Transform sideFrame_Left_transform;
    [SerializeField] Transform sideFrame_Right_transform;
    [SerializeField] Transform centerFrame_transform;
    [SerializeField] Transform gate_Left_transform;
    [SerializeField] Transform gate_Right_transform;

    [Space(10)]
    [SerializeField] float height_max = 5;

    [Space(10)]
    [SerializeField] float startHeight_left = 0;
    [SerializeField] float startHeight_right = 5;

    [Space(10)]
    [SerializeField] float gateSpeed = 0.1f;
    bool isLeftUp;
    bool isRightUp;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveGate_left();
        MoveGate_right();
    }

    void OnValidate()
    {
        sideFrame_Left_transform.localScale = new Vector3(1, height_max + 0.1f, 1);
        sideFrame_Right_transform.localScale = new Vector3(1, height_max + 0.1f, 1);
        centerFrame_transform.localScale = new Vector3(1, height_max + 0.1f, 1);

        gate_Left_transform.localScale = new Vector3(1, startHeight_left, 1);
        gate_Right_transform.localScale = new Vector3(1, startHeight_right, 1);
    }

    void MoveGate_left()
    {
        if (isLeftUp)
        {
            gate_Left_transform.localScale += new Vector3(0, gateSpeed * Time.fixedDeltaTime / 0.02f, 0);
        }
        else
        {
            gate_Left_transform.localScale -= new Vector3(0, gateSpeed * Time.fixedDeltaTime / 0.02f, 0);
        }


        if (gate_Left_transform.localScale.y > height_max)
        {
            gate_Left_transform.localScale = new Vector3(1, height_max, 1);
            isLeftUp = false;
        }
        else if (gate_Left_transform.localScale.y < 0)
        {
            gate_Left_transform.localScale = new Vector3(1, 0.1f, 1);
            isLeftUp = true;
        }
    }

    void MoveGate_right()
    {
        if (isRightUp)
        {
            gate_Right_transform.localScale += new Vector3(0, gateSpeed * Time.fixedDeltaTime / 0.02f, 0);
        }
        else
        {
            gate_Right_transform.localScale -= new Vector3(0, gateSpeed * Time.fixedDeltaTime / 0.02f, 0);
        }

        if (gate_Right_transform.localScale.y > height_max)
        {
            gate_Right_transform.localScale = new Vector3(1, height_max, 1);
            isRightUp = false;
        }
        else if (gate_Right_transform.localScale.y < 0)
        {
            gate_Right_transform.localScale = new Vector3(1, 0.1f, 1);
            isRightUp = true;
        }
    }
}
