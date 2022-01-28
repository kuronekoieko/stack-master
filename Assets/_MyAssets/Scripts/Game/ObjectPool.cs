using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour
{
    public List<Character> activelist = new List<Character>();
    List<Character> inActivelist = new List<Character>();
    Character prefab;
    int index;

    public void CreateInstance(Character prefab, int count, Action<Character> OnIntansiate)
    {
        this.prefab = prefab;
        for (int i = 0; i < count; i++)
        {
            Character c = Instantiate(prefab);
            c.gameObject.name = "character_" + index.ToString();
            index++;
            OnIntansiate(c);
            inActivelist.Add(c);
            c.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 控えのキャラをアクティブに、足りなかったら生成
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public void Activate(int count, out List<Character> activatedReserves, Action<Character> OnIntansiate)
    {
        activatedReserves = new List<Character>();

        CreateInstance(prefab, count - inActivelist.Count, OnIntansiate);


        for (int i = 0; i < count; i++)
        {
            var reserve = inActivelist[i];
            activatedReserves.Add(reserve);
            activelist.Add(reserve);
            reserve.gameObject.SetActive(true);
        }

        inActivelist.RemoveRange(0, count);
    }

    public void Remove(Character item)
    {
        if (inActivelist.Contains(item)) return;
        inActivelist.Add(item);
        activelist.Remove(item);
        // item.gameObject.SetActive(false);
    }
}
