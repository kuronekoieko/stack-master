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

    [SerializeField] ParticleSystem wallPs_Red;
    [SerializeField] ParticleSystem wallPs_Blue;
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
        wallPs_Blue.gameObject.SetActive(arithmeticOperator == ArithmeticOperator.Plus || arithmeticOperator == ArithmeticOperator.Multiplied);
        wallPs_Red.gameObject.SetActive(arithmeticOperator == ArithmeticOperator.Minus || arithmeticOperator == ArithmeticOperator.Divided);
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
