using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager i;
    AudioSource audioSource;

    void Awake()
    {
        if (i == null) i = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnStart()
    {
    }

    public void PlayOneShot(int resourceIndex)
    {
        if (SaveData.i.isOffSE) { return; }
        if (SoundResourceSO.i.resources.Length - 1 < resourceIndex) { return; }
        AudioClip clip = SoundResourceSO.i.resources[resourceIndex].audioClip;
        if (clip == null) { return; }
        audioSource.PlayOneShot(clip);
    }
}
