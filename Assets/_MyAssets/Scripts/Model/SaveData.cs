using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveData
{
    public static SaveData i => _i;
    private static SaveData _i = new SaveData();

    SaveData()
    {
        characterSkinSaveDatas = SkinSettingSO.i.CharacterSkinDatas.Select(h => new SkinSaveData(h.id, false)).ToList();
        characterSkinSaveDatas[0].isOwn = true;

        materialSkinSaveDatas = SkinSettingSO.i.characterMaterialDatas.Select(h => new SkinSaveData(h.id, false)).ToList();
        materialSkinSaveDatas[0].isOwn = true;
    }


    public bool isOffSE;
    public int currencyCount;
    public UserDateTime receivedLoginBonusUserDateTime;
    public int lastClearedDisplayStageNum = 0;
    public int selectedSkinIndex;
    public int selectedMaterialIndex;
    public List<SkinSaveData> characterSkinSaveDatas = new List<SkinSaveData>();
    public List<SkinSaveData> materialSkinSaveDatas = new List<SkinSaveData>();
    public UnlockingSkin unlockingSkin = new UnlockingSkin();
    public int startHumanCount = 1;
    public int offlineIncomeLevel = 1;
    public bool isFirstLaunch;
}

/// <summary>
/// datetimeは保存できない
/// </summary>
[System.Serializable]
public class UserDateTime
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;
}



[System.Serializable]
public class SkinSaveData
{
    public string id;
    public bool isOwn;

    public SkinSaveData(string id, bool isOwn)
    {
        this.id = id;
        this.isOwn = isOwn;
    }
}

[System.Serializable]
public class TestClass
{
    public string name;
    public int num;
}

[System.Serializable]
public class UnlockingSkin
{
    public int id;
    public int index;
    public int percentage;
}