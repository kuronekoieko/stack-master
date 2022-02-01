using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveFloorController : MonoBehaviour
{
    [SerializeField] Renderer belt_renderer;
    [SerializeField] bool isRight = true;
    [SerializeField] float speed = 0.1f;
    [SerializeField] float length = 1;
    [SerializeField] float width = 1;

    [Inject] CharacterManager characterManager;
    bool existCharacter = false;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        belt_renderer.material.SetFloat("_ScrollX", speed * 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!existCharacter) return;
        if (isRight) characterManager.pool.activelist[0].transform.position += new Vector3(speed * Time.fixedDeltaTime / 0.02f, 0, 0);
        else characterManager.pool.activelist[0].transform.position -= new Vector3(speed * Time.fixedDeltaTime / 0.02f, 0, 0);
    }

    void OnValidate()
    {
        transform.localScale = new Vector3(width, 1, length);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void OnTriggerStay(Collider collider)
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

        //characterManager.pool.activelist[0].
        existCharacter = true;
    }

    void OnExitPlayer(Collider collider)
    {
        if (!collider.TryGetComponent(out Character character)) return;

        existCharacter = false;
    }

    void MoveBelt()
    {

    }
}
