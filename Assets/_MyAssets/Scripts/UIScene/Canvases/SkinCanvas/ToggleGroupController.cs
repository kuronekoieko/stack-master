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
            tab.toggle.onValueChanged.AddListener((isOn) =>
            {
                tab.tabObj.SetActive(isOn);
                tab.toggle.image.color = isOn ? tab.toggle.colors.normalColor : tab.toggle.colors.disabledColor;
            });

            tab.tabObj.SetActive(tab.toggle.isOn);
            tab.toggle.image.color = tab.toggle.isOn ? tab.toggle.colors.normalColor : tab.toggle.colors.disabledColor;
        }
    }


}

[System.Serializable]
public class Tab
{
    public Toggle toggle;
    public GameObject tabObj;
}
