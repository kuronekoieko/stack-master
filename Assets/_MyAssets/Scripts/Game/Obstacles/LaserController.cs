using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] Transform laser_transform;
    [SerializeField] Transform muzzle_L_transform;
    [SerializeField] Transform muzzle_R_transform;
    [SerializeField] ParticleSystem spark_particleSystem_original;

    [Space(10)]
    [SerializeField] float length;
    [SerializeField] float width;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnValidate()
    {
        laser_transform.localScale = new Vector3(width, length, width);
        muzzle_L_transform.localPosition = new Vector3(length, 0, 0);
        muzzle_R_transform.localPosition = new Vector3(-length, 0, 0);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerEnter(Collider collider)
    {
        OnTouchPlayer(collider);
    }

    void OnTouchPlayer(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;

        ParticleSystem spark_particleSystem_clone = Instantiate(spark_particleSystem_original);
        spark_particleSystem_clone.transform.position = collider.transform.position;
        spark_particleSystem_clone.Play();
    }
}
