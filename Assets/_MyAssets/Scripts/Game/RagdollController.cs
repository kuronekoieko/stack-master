using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RagdollController : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Collider[] colliders;
    Rigidbody rb;
    Collider[] cols;
    Animator animator;
    private void Awake()
    {
        if (!Variables.isSkinReal) return;

        rb = GetComponent<Rigidbody>();
        cols = GetComponents<Collider>();
    }

    public void SetRagdoll(Animator animator)
    {
        if (!Variables.isSkinReal) return;
        this.animator = animator;
        rigidbodies = animator.transform.GetComponentsInChildren<Rigidbody>();
        colliders = animator.transform.GetComponentsInChildren<Collider>();
    }

    public void EnableRagdoll(bool enabled)
    {
        if (!Variables.isSkinReal) return;

        foreach (var r in rigidbodies)
        {
            r.isKinematic = !enabled;
        }

        foreach (var c in colliders)
        {
            c.enabled = enabled;
        }
        animator.enabled = !enabled;

        rb.isKinematic = enabled;
        foreach (var col in cols)
        {
            col.enabled = !enabled;
        }

        if (enabled)
        {
            ChangeLayersForAllChildren("Dead");
        }
        else
        {
            ChangeLayersForAllChildren("Character");
        }
    }

    void FixedUpdate()
    {
        if (!Variables.isSkinReal) return;
        if (animator.enabled) return;
        foreach (var r in rigidbodies)
        {
            r.AddForce(Vector3.down * 30f);
        }
    }

    void ChangeLayersForAllChildren(string layerName)
    {
        ChangeLayersForChildren(transform, layerName);
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }


    void ChangeLayersForChildren(Transform transform, string layerName)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
            ChangeLayersForChildren(child, layerName);
        }
    }
}
