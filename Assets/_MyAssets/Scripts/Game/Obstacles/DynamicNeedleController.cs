using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DynamicNeedleController : MonoBehaviour
{

    [System.Serializable]
    public struct NeedleEnable
    {
        public bool[] enable;
    }
    [SerializeField]
    NeedleEnable[] needleEnables = {
        new NeedleEnable()
    };
    [SerializeField] Transform[] needles_transform;

    [Space(10)]
    [SerializeField] float waitTimeOnDisappear_sec;
    [SerializeField] float appearCompleteTime_sec;
    [SerializeField] float waitTimeOnAppear_sec;
    [SerializeField] float disappearCompleteTime_sec;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NeedleSequence());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator NeedleSequence()
    {
        while (true)
        {
            for (int i = 0; i < needleEnables.Length; i++)
            {
                yield return new WaitForSeconds(waitTimeOnDisappear_sec);
                for (int j = 0; j < needles_transform.Length; j++)
                {
                    if (needleEnables[i].enable[j]) needles_transform[j].DOLocalMoveY(0, appearCompleteTime_sec);
                }
                yield return new WaitForSeconds(appearCompleteTime_sec + waitTimeOnAppear_sec);
                for (int j = 0; j < needles_transform.Length; j++)
                {
                    if (needleEnables[i].enable[j]) needles_transform[j].DOLocalMoveY(-1.1f, disappearCompleteTime_sec);
                }
            }
        }
    }
}
