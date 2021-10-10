using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider col;
    [SerializeField] ParticleSystem bloodPs;
    [SerializeField] SpriteRenderer inkSr;
    [SerializeField] Animator animator;
    float speedZ = 13f;
    float speedX = 30f;
    Vector3 vel;
    float currentVelocity;
    CharacterManager characterManager;
    public float Height => col.height;
    Vector3 inkScale;

    void Start()
    {
        inkScale = inkSr.transform.lossyScale;
        inkSr.gameObject.SetActive(false);
    }

    public void OnInstantiate(CharacterManager characterManager)
    {
        gameObject.SetActive(false);
        this.characterManager = characterManager;
        col.enabled = false;
    }

    public void Appear(Vector3 bottomPos, Vector3 targetPos, float duration)
    {
        rb.mass = 1f;
        gameObject.SetActive(true);
        transform.position = bottomPos;
        transform.DOMoveY(targetPos.y, duration)
        .OnComplete(() =>
        {
            col.enabled = true;
        });
    }

    public void VelocityControl(float deltax)
    {
        vel = rb.velocity;
        vel.z = speedZ;
        vel.x = Mathf.SmoothDamp(rb.velocity.x, deltax * speedX, ref currentVelocity, 0.1f);
        rb.velocity = vel;
        animator.SetBool("IsRun", rb.velocity.z > 0.1f);
        rb.mass = 1000f;
    }

    public void Follow(Vector3 bottomPos)
    {
        var pos = bottomPos;
        pos.y = rb.position.y;
        rb.position = pos;
        animator.SetBool("IsFall", true);
    }


    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterGate(other);

        if (other.CompareTag("Obstacle"))
        {
            Dead(other.ClosestPoint(transform.position), false);
        }
    }

    void OnTriggerEnterGate(Collider other)
    {
        var gate = other.gameObject.GetComponent<GateController>();
        if (gate == null) return;
        gate.OnHitCharacter();

        bool isIncrease = gate.ArithmeticOperator == ArithmeticOperator.Plus || gate.ArithmeticOperator == ArithmeticOperator.Multiplied;
        if (isIncrease)
        {
            int addCount = gate.ArithmeticOperator == ArithmeticOperator.Plus ? gate.Count : characterManager.ActiveCount * (gate.Count - 1);
            characterManager.AppearToStack(addCount);
        }
        else
        {
            int deadCount = gate.Count;
            if (gate.ArithmeticOperator == ArithmeticOperator.Divided) deadCount = (int)((float)characterManager.ActiveCount - (float)characterManager.ActiveCount / (float)gate.Count);
            characterManager.Dead(deadCount);
        }

    }

    public void Dead(Vector3 hitPos, bool isHitGate)
    {
        gameObject.SetActive(false);
        characterManager.Characters.Remove(this);

        bloodPs.transform.parent = null;
        var pos = transform.position;
        pos.y += Height / 2f;
        bloodPs.transform.position = pos;
        bloodPs.Play();

        if (isHitGate) return;

        inkSr.gameObject.SetActive(true);
        inkSr.transform.parent = null;
        inkSr.transform.localScale = Vector3.zero;
        hitPos.z -= 0.1f;
        inkSr.transform.position = hitPos;
        inkSr.transform.DOScale(inkScale, 0.5f);
    }
}
