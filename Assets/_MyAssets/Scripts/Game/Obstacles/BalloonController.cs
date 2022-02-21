using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

enum BalloonState
{
    Ready,
    Raise,
    Die
}
public class BalloonController : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion_particleSystem_original;
    [SerializeField] Collider explosionAttack;

    [Space(10)]
    [SerializeField] float balloonSize = 2;

    [Space(10)]
    [SerializeField] float blowUpTime_sec = 0.5f;
    [SerializeField] float raiseSpeed = 0.1f;
    [SerializeField] float vibrationWidth = 2;
    [SerializeField] float vibrationCycleTime_sec = 1.5f;
    [SerializeField] float lifeTime_sec = 7;

    [Space(10)]
    [SerializeField] float explosionContinuationTime_sec = 0.3f;

    BalloonState balloonState = BalloonState.Ready;
    float startX = 0;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Raise();
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerEnter(Collider collider)
    {
        OnTouch_Player(collider);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void Init(Vector3 pos)
    {
        balloonState = BalloonState.Ready;
        transform.position = pos;
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(balloonSize, balloonSize, balloonSize), blowUpTime_sec).SetEase(Ease.OutElastic);
        startX = transform.position.x;

        StartCoroutine(DelayMethod(blowUpTime_sec, () =>
        {
            balloonState = BalloonState.Raise;
            StartCoroutine(Vibrate());
        }));

        StartCoroutine(DelayMethod(blowUpTime_sec + lifeTime_sec, () =>
        {
            Explode();
        }));
    }

    void OnTouch_Player(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;

        Explode();
    }

    void Raise()
    {
        if (balloonState != BalloonState.Raise) return;
        transform.position += (Vector3.up * raiseSpeed * Time.deltaTime / 0.02f);
    }

    IEnumerator Vibrate()
    {
        float currentTime = 0;
        while (true)
        {
            Vector3 targetPos = transform.position;
            targetPos.x = startX + vibrationWidth / 2 * Mathf.Sin(2 * Mathf.PI * currentTime / vibrationCycleTime_sec);
            currentTime = (currentTime + Time.fixedDeltaTime) % vibrationCycleTime_sec;
            transform.position = targetPos;
            yield return new WaitForFixedUpdate();
        }
    }

    void Explode()
    {
        if (balloonState == BalloonState.Die) return;

        explosionAttack.transform.parent = null;
        explosionAttack.gameObject.SetActive(true);
        StartCoroutine(DelayMethod(explosionContinuationTime_sec, () => { explosionAttack.gameObject.SetActive(false); }));

        ParticleSystem explosion_particleSystem_clone = Instantiate(explosion_particleSystem_original);
        explosion_particleSystem_clone.transform.position = transform.position;
        explosion_particleSystem_clone.transform.localScale = new Vector3(balloonSize, balloonSize, balloonSize);
        explosion_particleSystem_clone.transform.parent = null;
        explosion_particleSystem_clone.Play();

        StopCoroutine(Vibrate());
        balloonState = BalloonState.Die;
        gameObject.SetActive(false);
    }


    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}
