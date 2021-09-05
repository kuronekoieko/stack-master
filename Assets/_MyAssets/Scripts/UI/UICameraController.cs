using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Unity CameraのcullingMaskについて
http://shinriyo.hateblo.jp/entry/20130208/p3
*/
public class UICameraController : MonoBehaviour
{
    [SerializeField] ParticleSystem confettiL;
    [SerializeField] ParticleSystem confettiR;
    public static UICameraController i;
    private void Awake()
    {
        i = this;
    }

    public void PlayConfetti()
    {
        confettiL.Play();
        confettiR.Play();
        SoundManager.i.PlayOneShot(1);
    }
}