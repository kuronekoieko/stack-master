using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [SerializeField] TextAsset defaultSaveDataJson;
    public static SaveDataManager i;

    void Awake()
    {
        if (i == null) i = this;
    }

    public void OnStart()
    {
        LoadSaveData();
    }

    public void Save()
    {
        if (defaultSaveDataJson == null)
        {
            Debug.Log("セーブデータの初期値がありません");
            return;
        }

        //ユーザーデータオブジェクトからjson形式のstringを取得
        string jsonStr = JsonUtility.ToJson(SaveData.i);

        SavePlayerPrefs(jsonStr);
    }

    void LoadSaveData()
    {
        if (defaultSaveDataJson == null)
        {
            Debug.Log("セーブデータの初期値がありません");
            return;
        }
        //初回起動時のユーザーデータ作成
        string defaultJsonStr = GetDefaultJsonStr();
        //PlayerPrefsに保存済みのユーザーデータのstringを取得
        //第二引数に初回起動時のデータを入れる
        string jsonStr = PlayerPrefs.GetString(Strings.KEY_SAVE_DATA, defaultJsonStr);
        //ユーザーデータオブジェクトに読み出したデータを格納
        //※このとき、新しく追加された変数は消されずマージされる
        JsonUtility.FromJsonOverwrite(jsonStr, SaveData.i);
        //アプデ対応(配列のサイズを追加するため)
        AddSaveDataInstance();
        //ユーザーデータ保存
        Save();
    }

    string GetDefaultJsonStr()
    {
        //ユーザーデータオブジェクトにデフォルトのjsonを書き込む
        //※アップデートで変数の種類が増えたときに初期値を入れておくため
        JsonUtility.FromJsonOverwrite(
            json: defaultSaveDataJson.text,
            objectToOverwrite: SaveData.i);

        //ユーザーデータオブジェクトの初期化
        InitSaveDataInstance();

        //デフォルトのユーザーデータを作成
        string defaultJsonStr = JsonUtility.ToJson(SaveData.i);
        return defaultJsonStr;
    }

    void SavePlayerPrefs(string jsonStr)
    {
        //jsonデータをセットする
        PlayerPrefs.SetString(Strings.KEY_SAVE_DATA, jsonStr);
        //保存する
        PlayerPrefs.Save();
    }

    void InitSaveDataInstance()
    {
    }

    void AddSaveDataInstance()
    {
    }

}
