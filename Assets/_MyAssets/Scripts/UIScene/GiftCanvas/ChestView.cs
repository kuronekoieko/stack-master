using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour
{
    [SerializeField] Button button;
    public void OnStart()
    {
        button.onClick.AddListener(OnClickChestButton);
    }

    void OnClickChestButton()
    {

    }


}
