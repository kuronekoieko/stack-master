using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ArithmeticOperator
{
    Plus,
    Minus,
    Multiplied,
    Divided,
}

public class GateController : MonoBehaviour
{
    [SerializeField] ArithmeticOperator arithmeticOperator;
    [SerializeField] int count;
    [SerializeField] TextMeshPro textMeshPro;
    [SerializeField] Collider col;
    GateManager gateManager;
    Dictionary<ArithmeticOperator, string> arithmeticOperatorStrings = new Dictionary<ArithmeticOperator, string>
    {
        {ArithmeticOperator.Plus,"+"},
        {ArithmeticOperator.Minus,"-"},
        {ArithmeticOperator.Multiplied,"x"},
        {ArithmeticOperator.Divided,"/"},
    };

    public ArithmeticOperator ArithmeticOperator => arithmeticOperator;
    public int Count => count;
    public Collider Col => col;

    void OnValidate()
    {
        textMeshPro.text = arithmeticOperatorStrings[arithmeticOperator] + count;
    }

    void Awake()
    {
        gateManager = GetComponentInParent<GateManager>();
    }

    public void OnHitCharacter()
    {
        gateManager.EnableGatesCollider(false);
    }
}
