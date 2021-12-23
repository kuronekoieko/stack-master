using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create SoundResourceSO", fileName = "SoundResourceSO")]
public class SoundResourceSO : ScriptableObject
{
    public SoundResource[] resources;

    public static SoundResourceSO i { get; set; }
}

[System.Serializable]
public class SoundResource
{
    public AudioClip audioClip;
    public string name;
}