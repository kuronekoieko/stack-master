using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public static SaveData i => _i;
    private static SaveData _i = new SaveData();
    public bool isOffSE;
    public int coinCount;
    public UserDateTime receivedLoginBonusUserDateTime;
    public int lastClearedDisplayStageNum = 0;
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