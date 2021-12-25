using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableElement : MonoBehaviour
{
    public TableElement tableElement_previous;
    public bool isMoving = false;

    public void Move(float moveSpeed)
    {
        if (tableElement_previous.transform.localPosition.x > -3.4f) isMoving = true;
        if (!isMoving) return;

        if (tableElement_previous.isMoving) transform.localPosition = tableElement_previous.transform.localPosition - new Vector3(1.45f, 0, 0);
        else transform.localPosition += new Vector3(moveSpeed, 0, 0);

        if (transform.localPosition.x > 5)
        {
            isMoving = false;
            transform.localPosition = new Vector3(-5, 0, 0);
        }
    }
}
