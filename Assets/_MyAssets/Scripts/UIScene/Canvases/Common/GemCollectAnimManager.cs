using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemCollectAnimManager : MonoBehaviour
{
    [SerializeField] GemCollectAnim gemCollectAnimPrefab;
    GemCollectAnim[] gemCollectAnims;

    public void OnStart(int gemImageCount)
    {
        gemCollectAnims = new GemCollectAnim[gemImageCount];
        for (int i = 0; i < gemCollectAnims.Length; i++)
        {
            gemCollectAnims[i] = Instantiate(gemCollectAnimPrefab, transform);
            gemCollectAnims[i].OnInstansiate();
        }
    }

    public void Anim(Vector3 startPos, float width, Action OnMoveEnd = null)
    {
        Vector3 startOffset = Vector3.zero;
        for (int i = 0; i < gemCollectAnims.Length; i++)
        {
            startOffset.x = UnityEngine.Random.Range(-width, width);
            startOffset.y = UnityEngine.Random.Range(-width, width);
            float delay = UnityEngine.Random.Range(0, 0.2f);

            if (i == 0)
            {
                gemCollectAnims[i].Anim(startPos, startOffset, CoinCountView.i.GemImagePos, delay, OnMoveEnd);
            }
            else
            {
                gemCollectAnims[i].Anim(startPos, startOffset, CoinCountView.i.GemImagePos, delay);
            }
        }
    }
}
