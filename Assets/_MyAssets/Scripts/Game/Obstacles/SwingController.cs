using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    [SerializeField] Transform axe_transform;
    [SerializeField] Transform axe_body_transform;
    [SerializeField] Transform axe_bar_transform;

    [Space(10)]
    [SerializeField] float length = 2.6f;

    [Space(10)]
    [SerializeField] float range = 90;
    [SerializeField] float cycleTime_sec = 4;
    [SerializeField] float offsetTime_sec = 0;

    float latestStartTime_sec;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Swing());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnValidate()
    {
        axe_body_transform.localPosition = new Vector3(0, -length + 0.6f, 0);
        axe_bar_transform.localScale = new Vector3(0.1f, length, 0.06f);
        axe_bar_transform.localPosition = new Vector3(0, -length / 2, 0);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator Swing()
    {
        float currentTime = offsetTime_sec;
        while (true)
        {
            currentTime += Time.fixedDeltaTime;
            currentTime %= cycleTime_sec;
            axe_transform.localEulerAngles = new Vector3(0, 0, range / 2 * Mathf.Sin(2 * Mathf.PI * currentTime / cycleTime_sec));
            yield return new WaitForFixedUpdate();
        }
    }
}
