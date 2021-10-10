using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider col;
    float speedZ = 10f;
    float speedX = 20f;
    Vector3 vel;
    float currentVelocity;
    CharacterManager characterManager;
    public float Height => col.height;
    void Start()
    {

    }

    public void OnInstantiate(CharacterManager characterManager)
    {
        gameObject.SetActive(false);
        this.characterManager = characterManager;
    }

    public void Appear(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
    }


    void Update()
    {

    }


    public void VelocityControl(float deltax)
    {
        vel = rb.velocity;
        vel.z = speedZ;
        vel.x = Mathf.SmoothDamp(rb.velocity.x, deltax * speedX, ref currentVelocity, 0.1f);
        rb.velocity = vel;
    }

    public void Follow(Vector3 bottomPos)
    {
        var pos = bottomPos;
        pos.y = rb.position.y;
        rb.position = pos;
    }


    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterGate(other);

        if (other.CompareTag("Obstacle"))
        {
            Dead();
        }
    }

    void OnTriggerEnterGate(Collider other)
    {
        var gate = other.gameObject.GetComponent<GateController>();
        if (gate == null) return;
        gate.OnHitCharacter();
        characterManager.AppearToStack(gate.Count);
    }

    void Dead()
    {
        gameObject.SetActive(false);
        characterManager.Characters.Remove(this);
    }
}
