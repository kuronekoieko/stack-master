using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBevelController : MonoBehaviour
{
    [SerializeField] TableElement tableElement_original;
    [SerializeField] GameObject bevel_gameObject_original;
    [SerializeField] Transform moveTable_transform;

    [Space(10)]
    [SerializeField, TextArea(3, 5)] string document;
    [SerializeField] List<int> bevelCount;
    [SerializeField] float moveSpeed = 0.1f;
    TableElement[] tableElements;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        if (bevelCount.Count >= 8)
        {
            tableElements = new TableElement[bevelCount.Count];
        }
        else
        {
            tableElements = new TableElement[8];
            while (bevelCount.Count < 8)
            {
                bevelCount.Add(0);
            }
        }

        for (int i = 0; i < tableElements.Length; i++)
        {
            tableElements[i] = Instantiate(tableElement_original);
            tableElements[i].transform.parent = moveTable_transform;
            tableElements[i].transform.localPosition = new Vector3(-5, 0, 0);
            for (int j = 0; j < bevelCount[i]; j++)
            {
                GameObject bevel_gameObject_clone = Instantiate(bevel_gameObject_original);
                bevel_gameObject_clone.transform.parent = tableElements[i].transform;
                bevel_gameObject_clone.transform.localPosition = new Vector3(0, 0.7f + 1.45f * j, 0);
            }
        }
        for (int i = 0; i < tableElements.Length; i++)
        {
            int num = i - 1;
            if (num < 0) num = tableElements.Length - 1;
            tableElements[i].tableElement_previous = tableElements[num];
        }

        tableElements[0].transform.localPosition = new Vector3(2.9f, 0, 0);
        for (int i = 0; i < 5; i++) tableElements[i].isMoving = true;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < tableElements.Length; i++)
        {
            tableElements[i].Move(moveSpeed);
        }
    }
}
