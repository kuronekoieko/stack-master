using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager i;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource deadAudioSource;
    bool isInterval;

    void Awake()
    {
        i = this;
    }


    public void PlayOneShot(int resourceIndex)
    {
        if (SaveData.i.isOffSE) { return; }
        if (SoundResourceSO.i.resources.Length - 1 < resourceIndex) { return; }
        AudioClip clip = SoundResourceSO.i.resources[resourceIndex].audioClip;
        if (clip == null) { return; }
        audioSource.PlayOneShot(clip);
    }


    public void PlayOneShotDead()
    {
        if (isInterval) return;
        deadAudioSource.PlayOneShot(SoundResourceSO.i.resources[2].audioClip);
        isInterval = true;
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ =>
        {
            isInterval = false;
        });
    }

}
