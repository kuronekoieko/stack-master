using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T i
    {
        get
        {
            if (_i == null) _i = FindObjectOfType<T>();
            return _i;
        }
    }
    private static T _i;
}
