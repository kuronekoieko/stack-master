using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create SoundResourceSO", fileName = "SoundResourceSO")]
public class SoundResourceSO : ScriptableObject
{
    public SoundResource[] resources;

    private static SoundResourceSO _i;
    public static SoundResourceSO i
    {
        get
        {
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
    }
}

[System.Serializable]
public class SoundResource
{
    public AudioClip audioClip;
    public string name;
}