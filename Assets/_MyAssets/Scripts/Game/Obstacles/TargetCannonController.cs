using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TargetCannonController : MonoBehaviour
{
    [SerializeField] bool istest;

    [SerializeField] Transform cannonBody_transform;
    [SerializeField] Transform muzzle_transform;
    [SerializeField] Transform laser_transform;
    [SerializeField] Transform rangeTrigger;
    [SerializeField] ParticleSystem smoke_particleSystem;
    [SerializeField] ExplosionBulletController explosionBullet_original;

    [Space(10)]
    [SerializeField] float searchTime_sec;
    [SerializeField] float shootLagTime_sec;
    [SerializeField] float rechargeTime_sec;

    [Space(10)]
    [SerializeField] float shootPower = 1000;

    [Space(10)]
    [SerializeField] float laserWidth = 0.2f;
    [SerializeField] float range = 25;

    [Inject] CharacterManager characterManager;
    float latestStartCycleTime_sec;
    bool isFirstSmoke = true;
    bool isActive = false;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        //characterManager = TargetCannonManager.i.characterManager;
        latestStartCycleTime_sec = Time.fixedTime;
        laser_transform.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckActive();
    }

    void FixedUpdate()
    {
        CannonRoutine();
    }

    void OnValidate()
    {
        laser_transform.localScale = new Vector3(1, laserWidth, laserWidth);
        rangeTrigger.localScale = new Vector3(range, 1, range);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerEnter(Collider collider)
    {
        OnTouchPlayer(collider);
    }

    void OnTriggerExit(Collider collider)
    {
        OnExitPlayer(collider);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTouchPlayer(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;
        latestStartCycleTime_sec = Time.fixedTime;
        isActive = true;
        laser_transform.gameObject.SetActive(true);
    }

    void OnExitPlayer(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;
        isActive = false;
        laser_transform.gameObject.SetActive(false);
    }


    void CannonRoutine()
    {
        if (!isActive) return;
        Rotate_this2chara();
        ShootLag();
        Shoot();
        RechargeWait();
    }

    void Rotate_this2chara()
    {
        //if (cannonState != CannonState.Search) return;
        if (Time.fixedTime - latestStartCycleTime_sec > searchTime_sec) return;
        if (characterManager.pool.activelist.Count <= 0) return;

        float lerpRatio = 0.2f;
        Vector3 targetPos = Vector3.Lerp(transform.position + transform.forward, characterManager.pool.activelist[0].transform.position + new Vector3(0, 0, 4), lerpRatio * 0.02f / Time.fixedDeltaTime);
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
        targetPos = Vector3.Lerp(transform.position + transform.forward, characterManager.pool.activelist[0].transform.position + new Vector3(0, 0, 4), lerpRatio * 0.02f / Time.fixedDeltaTime);
        cannonBody_transform.LookAt(targetPos);
        ChangeLaserLength();
    }

    void ChangeLaserLength()
    {
        RaycastHit hit;
        if (Physics.Raycast(muzzle_transform.position, cannonBody_transform.forward, out hit))
        {
            float distance = Vector3.Distance(muzzle_transform.position, hit.point) / 2;
            laser_transform.localScale = new Vector3(distance, laserWidth, laserWidth);
        }
    }

    void ShootLag()
    {
        if (characterManager.playerState != PlayerState.Playing) return;
        if (Time.fixedTime - latestStartCycleTime_sec < searchTime_sec) return;
        laser_transform.gameObject.SetActive(false);
    }

    void Shoot()
    {
        //if (characterManager.playerState != PlayerState.Playing) return;
        if (characterManager.playerState != PlayerState.Playing) return;
        if (Time.fixedTime - latestStartCycleTime_sec < searchTime_sec + shootLagTime_sec) return;
        if (!isFirstSmoke) return;

        smoke_particleSystem.Play();
        isFirstSmoke = false;
        ExplosionBulletController explosionBullet_clone = Instantiate(explosionBullet_original);
        explosionBullet_clone.transform.position = muzzle_transform.position;
        explosionBullet_clone.Shoot(cannonBody_transform.forward * shootPower);
    }

    void RechargeWait()
    {
        if (Time.fixedTime - latestStartCycleTime_sec < searchTime_sec + shootLagTime_sec + rechargeTime_sec) return;

        latestStartCycleTime_sec = Time.fixedTime;
        isFirstSmoke = true;
        laser_transform.gameObject.SetActive(true);
    }

    void CheckActive()
    {
        if (characterManager.pool.activelist.Count <= 0)
        {
            isActive = false;
            laser_transform.gameObject.SetActive(false);
            return;
        }

        if (characterManager.playerState != PlayerState.Playing) return;
        float distance = Vector3.Distance(rangeTrigger.position, characterManager.pool.activelist[0].transform.position);
        if (distance < range / 2) return;
        if (!istest) return;
        
        isActive = false;
        laser_transform.gameObject.SetActive(false);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}
