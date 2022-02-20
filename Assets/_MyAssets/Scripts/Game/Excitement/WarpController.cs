using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpController : MonoBehaviour
{
    [SerializeField] Transform entranceHole_transform;
    [SerializeField] Transform exitHole_transform;
    [SerializeField] ParticleSystem entranceHole_particleSystem;
    [SerializeField] ParticleSystem exitHole_particleSystem;

    [Space(10)]
    [SerializeField] Vector3 entrancePosition;
    [SerializeField] Vector3 exitPosition;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        //entranceHole_transform.GetComponent<Renderer>().enabled = false;
        //exitHole_transform.GetComponent<Renderer>().enabled = false;
    }

    void OnValidate()
    {
        entranceHole_transform.localPosition = entrancePosition;
        entranceHole_particleSystem.transform.localPosition = entrancePosition;
        exitHole_transform.localPosition = exitPosition;
        exitHole_particleSystem.transform.localPosition = exitPosition;
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerEnter(Collider collider)
    {
        OnTouch_Player(collider);
    }

    void OnTouch_Player(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;

        character.transform.position = exitHole_transform.position;
        StartCoroutine(WarpAgain(character));
    }

    IEnumerator WarpAgain(Character character)
    {
        yield return null;
        character.transform.position = exitHole_transform.position;
    }
}
