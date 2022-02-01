using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBulletController : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion_particleSystem_original;
    [SerializeField] Collider hitTrigger;
    [SerializeField] float explosionScale = 4;
    [SerializeField] float deleteTime_sec = 5;
    Rigidbody _rigidbody;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        StartCoroutine(DelayMethod(deleteTime_sec, () =>
        {
            gameObject.SetActive(false);
        }));
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    public void Shoot(Vector3 direction)
    {
        _rigidbody.AddForce(direction);
    }

    void Explosion()
    {
        ParticleSystem explosion_particleSystem_clone = Instantiate(explosion_particleSystem_original);
        explosion_particleSystem_clone.transform.position = transform.position;
        explosion_particleSystem_clone.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);
        explosion_particleSystem_clone.Play();

        _rigidbody.velocity = Vector3.zero;
        hitTrigger.gameObject.SetActive(true);
        hitTrigger.transform.localScale = new Vector3(explosionScale * 1.5f, explosionScale * 1.5f, explosionScale * 1.5f);
        StartCoroutine(DelayMethod(0.1f, () => { gameObject.SetActive(false); }));
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}
