using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T m_Instance;

    /// <summary>
    /// 初回呼び出し時にResources.Loadが走るので、Unirxで監視したりしてるとスプラッシュ前に重かったりするので注意
    /// </summary>
    /// <value></value>
    public static T Instance
    {
        get
        {
            if (m_Instance) return m_Instance;
            Debug.Log("iiiiiiiiiiiiii");
            string PATH = "ScriptableObjects/" + typeof(T).Name;
            m_Instance = Resources.Load<T>(PATH);
            //ロード出来なかった場合はエラーログを表示
            if (m_Instance == null)
            {
                Debug.LogError(PATH + " not found");
            }

            return m_Instance;
        }
    }

    public void SetInstance(T instance)
    {
        if (m_Instance) return;
        Debug.Log("aaaaaaaaaaaa");
        m_Instance = instance;
    }
}
