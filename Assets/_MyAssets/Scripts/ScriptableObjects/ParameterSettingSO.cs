using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create ParameterSettingSO", fileName = "ParameterSettingSO")]
public class ParameterSettingSO : ScriptableObject
{
    public static ParameterSettingSO i;
}

[System.Serializable]
public class GiftRewardData
{
    public int probability;
    public int rewardCurrency;
}