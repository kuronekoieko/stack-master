using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public static SaveData i => _i;
    private static SaveData _i = new SaveData();
    public bool isOffSE;
    public int currencyCount;
    public UserDateTime receivedLoginBonusUserDateTime;
    public int lastClearedDisplayStageNum = 0;
    public int selectedSkinIndex;
    public CharacterSkinSaveData[] characterSkinSaveDatas;
    // public TestClass[] testClasses;

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
public class CharacterSkinSaveData
{
    public string id;
    public bool isOwn;

    public CharacterSkinSaveData(string id, bool isOwn)
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