using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupController : MonoBehaviour
{
    [SerializeField] Tab[] tabs;
    void Start()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            var tab = tabs[i];
            tab.toggle.onValueChanged.AddListener((isOn) => OnChangedToggleValue(tab, isOn));
            tab.tabObj.SetActive(tab.toggle.isOn);
            tab.toggle.image.color = tab.toggle.isOn ? tab.toggle.colors.normalColor : tab.toggle.colors.disabledColor;
        }

        if (Variables.isSkinReal)
        {
            tabs[0].tabObj.SetActive(true);
            tabs[0].toggle.gameObject.SetActive(false);
            tabs[1].tabObj.SetActive(false);
            tabs[1].toggle.gameObject.SetActive(false);
        }
    }

    void OnChangedToggleValue(Tab tab, bool isOn)
    {
        tab.tabObj.SetActive(isOn);
        tab.toggle.image.color = isOn ? tab.toggle.colors.normalColor : tab.toggle.colors.disabledColor;
    }
}

[System.Serializable]
public class Tab
{
    public Toggle toggle;
    public GameObject tabObj;
}
