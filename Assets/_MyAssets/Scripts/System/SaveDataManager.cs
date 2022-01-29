using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveDataManager
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
        //初回起動時のユーザーデータ作成(コンストラクタで初期値設定)
        string defaultJsonStr = JsonUtility.ToJson(SaveData.i);
        //PlayerPrefsに保存済みのユーザーデータのstringを取得
        //第二引数に初回起動時のデータを入れる
        string jsonStr = PlayerPrefs.GetString(Strings.KEY_SAVE_DATA, defaultJsonStr);
        //ユーザーデータオブジェクトに読み出したデータを格納
        //※このとき、新しく追加された変数は消されずマージされる
        JsonUtility.FromJsonOverwrite(jsonStr, SaveData.i);
        //アプデ対応(配列のサイズを追加するため)
        AddNewArrayElements();
        //ユーザーデータ保存
        Save();
    }

    /// <summary>
    /// アプデで配列が増えてたときに追加する
    /// </summary>
    void AddNewArrayElements()
    {
        // TODO:あとで新しいIDを追加するように変更
        for (int i = SaveData.i.characterSkinSaveDatas.Count; i < SkinSettingSO.i.CharacterSkinDatas.Length; i++)
        {
            var characterSkinData = SkinSettingSO.i.CharacterSkinDatas[i];
            SaveData.i.characterSkinSaveDatas.Add(new SkinSaveData(characterSkinData.id, false));
        }

        for (int i = SaveData.i.materialSkinSaveDatas.Count; i < SkinSettingSO.i.characterMaterialDatas.Length; i++)
        {
            var characterSkinData = SkinSettingSO.i.characterMaterialDatas[i];
            SaveData.i.materialSkinSaveDatas.Add(new SkinSaveData(characterSkinData.id, false));
        }
    }
}
