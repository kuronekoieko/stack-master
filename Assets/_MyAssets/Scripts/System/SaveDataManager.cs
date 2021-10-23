using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager i => _i;
    private static SaveDataManager _i = new SaveDataManager();


    public void Save()
    {
        //ユーザーデータオブジェクトからjson形式のstringを取得
        string jsonStr = JsonUtility.ToJson(SaveData.i);
        //jsonデータをセットする
        PlayerPrefs.SetString(Strings.KEY_SAVE_DATA, jsonStr);
        //保存する
        PlayerPrefs.Save();
    }

    public void LoadSaveData()
    {
        //初回起動時のユーザーデータ作成
        //string defaultJsonStr = GetDefaultJsonStr();
        //PlayerPrefsに保存済みのユーザーデータのstringを取得
        //第二引数に初回起動時のデータを入れる
        string jsonStr = PlayerPrefs.GetString(Strings.KEY_SAVE_DATA);
        //ユーザーデータオブジェクトに読み出したデータを格納
        //※このとき、新しく追加された変数は消されずマージされる
        JsonUtility.FromJsonOverwrite(jsonStr, SaveData.i);
        //アプデ対応(配列のサイズを追加するため)
        AddSaveDataInstance();
        //ユーザーデータ保存
        Save();
    }

    void InitSaveDataInstance()
    {
    }

    void AddSaveDataInstance()
    {
    }

}
