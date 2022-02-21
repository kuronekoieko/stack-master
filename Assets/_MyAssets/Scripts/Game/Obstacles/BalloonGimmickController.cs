using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonGimmickController : MonoBehaviour
{
    [SerializeField] Renderer spawnerAnchor_renderer;
    [SerializeField] BalloonController balloon_original;

    [Space(10)]
    [SerializeField] float startOffsetTime_sec = 0;
    [SerializeField] float spawnTime_sec;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        spawnerAnchor_renderer.enabled = false;
        StartCoroutine(MakeBalloon());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator MakeBalloon()
    {
        yield return new WaitForSeconds(startOffsetTime_sec);
        while (true)
        {
            BalloonController balloon_clone = Instantiate(balloon_original);
            balloon_clone.Init(transform.position);
            yield return new WaitForSeconds(spawnTime_sec);
        }
    }
}
