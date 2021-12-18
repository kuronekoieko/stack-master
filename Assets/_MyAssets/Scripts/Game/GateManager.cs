using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    GateController[] gates;

    void Awake()
    {
        gates = GetComponentsInChildren<GateController>();
        for (int i = 0; i < gates.Length; i++)
        {
            gates[i].OnAwake(this);
        }
    }

    public void EnableGatesCollider(bool enabled)
    {
        foreach (var item in gates)
        {
            item.Col.enabled = enabled;
        }
    }
}
