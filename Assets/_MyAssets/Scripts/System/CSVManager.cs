using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVManager : SingletonMonoBehaviour<CSVManager>
{
    [SerializeField] TextAsset level_reward_table;
    [SerializeField] TextAsset character_skin_price_table;
    [SerializeField] TextAsset material_skin_price_table;

    public List<LevelReward> LevelRewardTable;
    public List<SkinPrice> CharacterSkinPrices;
    public List<SkinPrice> MaterialSkinPrices;

    public void ParseCSV()
    {
        LevelRewardTable = GetTableFromCSV<LevelReward>(level_reward_table);
        CharacterSkinPrices = GetTableFromCSV<SkinPrice>(character_skin_price_table);
        MaterialSkinPrices = GetTableFromCSV<SkinPrice>(material_skin_price_table);
    }

    List<T> GetTableFromCSV<T>(TextAsset csvFile) where T : ICSVData<T>, new()
    {
        var table = new List<T>();
        var strTable = GetStringTableFromCSV(csvFile);

        //先頭行はカラム名なので飛ばす
        for (int row = 1; row < strTable.Count; row++)
        {
            T column = new T();
            column.SetParsedInstance(strTable[row]);
            table.Add(column);
        }
        return table;
    }

    List<string[]> GetStringTableFromCSV(TextAsset csvFile)
    {
        var strList = new List<string[]>();
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            strList.Add(line.Split(',')); // , 区切りでリストに追加
        }
        return strList;
    }
}


public class LevelReward : ICSVData<LevelReward>
{
    public int level;
    public int clearReward;
    public int chestsReward;

    public void SetParsedInstance(string[] strColumn)
    {
        int.TryParse(strColumn[0], out level);
        int.TryParse(strColumn[1], out clearReward);
        int.TryParse(strColumn[2], out chestsReward);
    }
}

public class SkinPrice : ICSVData<SkinPrice>
{
    public int numberOfPurchase;
    public int price;

    public void SetParsedInstance(string[] strColumn)
    {
        int.TryParse(strColumn[0], out numberOfPurchase);
        int.TryParse(strColumn[1], out price);
    }
}


public interface ICSVData<T>
{
    void SetParsedInstance(string[] strColumn);
}