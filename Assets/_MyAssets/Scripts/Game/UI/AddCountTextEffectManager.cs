using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCountTextEffectManager : SingletonMonoBehaviour<AddCountTextEffectManager>
{
    [SerializeField] AddCountTextEffect addCountTextEffectPrefab;
    public RectTransform canvasRt;
    public Camera uiCamera;
    List<AddCountTextEffect> addCountTextEffects = new List<AddCountTextEffect>();


    public void OnStart()
    {
        for (int i = 0; i < 20; i++)
        {
            var addCountTextEffect = Instantiate(addCountTextEffectPrefab, transform);
            addCountTextEffect.OnInstantiate();
            addCountTextEffects.Add(addCountTextEffect);
        }
    }

    public void Show(int count, Transform worldPos)
    {
        for (int i = 0; i < addCountTextEffects.Count; i++)
        {
            if (addCountTextEffects[i].gameObject.activeSelf) continue;
            addCountTextEffects[i].target = worldPos;
            addCountTextEffects[i].Show(count);
            return;
        }
        var addCountTextEffect = Instantiate(addCountTextEffectPrefab, transform);
        addCountTextEffect.OnInstantiate();
        addCountTextEffects.Add(addCountTextEffect);
        addCountTextEffect.target = worldPos;
        addCountTextEffect.Show(count);
    }

}
