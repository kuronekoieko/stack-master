using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    GateController[] gates;

    void Awake()
    {
        gates = GetComponentsInChildren<GateController>();
    }

    public void EnableGatesCollider(bool enabled)
    {
        foreach (var item in gates)
        {
            item.Col.enabled = enabled;
        }
    }
}
