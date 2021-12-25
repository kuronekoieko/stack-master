using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create SoundResourceSO", fileName = "SoundResourceSO")]
public class SoundResourceSO : ScriptableObject
{
    public SoundResource[] resources;

    static SoundResourceSO _i;
    public static SoundResourceSO i
    {
        get
        {
            if (Variables.isLaunchUIScene) return _i;
            string PATH = "ScriptableObjects/" + nameof(SoundResourceSO);
            //初アクセス時にロードする
            if (_i == null)
            {
                _i = Resources.Load<SoundResourceSO>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if (_i == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _i;
        }
        set { _i = value; }
    }
}

[System.Serializable]
public class SoundResource
{
    public AudioClip audioClip;
    public string name;
}