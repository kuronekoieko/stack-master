using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlagPointController : MonoBehaviour
{
    [SerializeField] Transform flag_transform;
    [SerializeField] GameObject flagTrigger;
    [SerializeField] ParticleSystem confetti_left_particleSystem;
    [SerializeField] ParticleSystem confetti_right_particleSystem;

    [Space(10)]
    [SerializeField] float flagUpTime_sec;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        OnTouchPlayer(collider);
    }

    void OnTouchPlayer(Collider collider)
    {
        if (!collider.TryGetComponent(out Character chr)) return;
        flagTrigger.SetActive(false);
        flag_transform.DOLocalRotate(new Vector3(0, 0, 0), flagUpTime_sec);
        confetti_left_particleSystem.Play();
        confetti_right_particleSystem.Play();
    }
}
