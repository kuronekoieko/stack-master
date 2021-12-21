using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtentions
{
    public static T GetRandom<T>(this List<T> self, out int index)
    {
        index = Random.Range(0, self.Count);
        return self[index];
    }

    public static T GetRandom<T>(this List<T> self)
    {
        int index = Random.Range(0, self.Count);
        return self[index];
    }

    public static T GetRandom<T>(this T[] self, out int index)
    {
        index = Random.Range(0, self.Length);
        return self[index];
    }

    public static T GetRandom<T>(this T[] self)
    {
        int index = Random.Range(0, self.Length);
        return self[index];
    }


    public static bool TryGetValue<T>(this T[] self, int index, out T value)
    {
        if (self.IsIndexOutOfRange<T>(index))
        {
            value = default;
            return false;
        }
        else
        {
            value = self[index];
            return true;
        }
    }

    public static bool IsIndexOutOfRange<T>(this T[] self, int index)
    {
        return index < 0 || self.Length <= index;
    }

    public static bool TryGetValue<T>(this List<T> self, int index, out T value)
    {
        if (self.IsIndexOutOfRange<T>(index))
        {
            value = default;
            return false;
        }
        else
        {
            value = self[index];
            return true;
        }
    }

    public static bool IsIndexOutOfRange<T>(this List<T> self, int index)
    {
        return index < 0 || self.Count <= index;
    }
}
