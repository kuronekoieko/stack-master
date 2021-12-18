using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
using UniRx;

public class Character : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] ParticleSystem bloodPs;
    [SerializeField] SpriteRenderer inkSr;
    [SerializeField] Animator animator;
    [Inject] CameraController cameraController;
    float speedZ = 15f;
    Vector3 vel;
    float currentVelocity;
    CharacterManager characterManager;
    public float Height => capsuleCollider.height;
    Vector3 inkScale;
    ParticleSystem[] bloodPsChildren;

    void Awake()
    {
        bloodPsChildren = bloodPs.GetComponentsInChildren<ParticleSystem>();
        this.ObserveEveryValueChanged(_ => SaveData.i.selectedSkinIndex)
            .Subscribe(_ => OnChangedSkin(_));

        this.ObserveEveryValueChanged(_ => SaveData.i.selectedMaterialIndex)
            .Subscribe(_ => OnChangedMaterial(_));
    }

    void OnChangedSkin(int selectedSkinIndex)
    {
        SkinController skinController = Instantiate(SkinSettingSO.i.characterSkinDatas[selectedSkinIndex].prefab, transform);
        skinController.OnInstantiate();
        DestroyImmediate(animator.gameObject);
        animator = skinController.Animator;
    }

    void OnChangedMaterial(int selectedIndex)
    {
        inkSr.material = new Material(SkinSettingSO.i.characterMaterialDatas[selectedIndex].material);
        for (int i = 0; i < bloodPsChildren.Length; i++)
        {
            ParticleSystem.MainModule main = bloodPsChildren[i].main;
            main.startColor = inkSr.material.color;
        }
    }

    void Start()
    {
        inkScale = inkSr.transform.lossyScale;
        inkSr.gameObject.SetActive(false);
        gameObject.AddComponent<ZenAutoInjecter>();// awakeはだめっぽい
    }

    public void OnInstantiate(CharacterManager characterManager)
    {
        gameObject.SetActive(false);
        this.characterManager = characterManager;
        capsuleCollider.enabled = false;
        boxCollider.enabled = false;
    }

    public void Appear(Vector3 bottomPos, Vector3 targetPos, float duration)
    {
        rb.mass = 1f;
        gameObject.SetActive(true);
        transform.position = bottomPos;
        transform.DOMoveY(targetPos.y, duration)
        .OnComplete(() =>
        {
            capsuleCollider.enabled = true;
            boxCollider.enabled = true;
            if (characterManager.Characters[0] != this) SoundManager.i?.PlayOneShot(0);
        });
    }

    public void VelocityControl(float deltax)
    {
        vel = rb.velocity;
        vel.z = speedZ;
        vel.x = Mathf.SmoothDamp(rb.velocity.x, deltax * Variables.speedX, ref currentVelocity, Variables.smoothTimeX);
        if (float.IsNaN(vel.x)) vel.x = 0;
        rb.velocity = vel;
        animator.SetBool("IsRun", rb.velocity.z > 0.1f);
        rb.mass = 1000f;
    }

    public void Follow(Vector3 bottomPos)
    {
        if (!gameObject.activeSelf) return;
        var pos = bottomPos;
        pos.y = rb.position.y;
        rb.position = pos;
        animator.SetBool("IsFall", true);
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    public void Dance()
    {
        transform.forward = -transform.forward;
        animator.SetBool("IsDance", true);
    }

    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterGate(other);
        OnTriggerEnterGoal(other);
        OnTriggerEnterGoalStair(other);
        if (other.CompareTag("Obstacle"))
        {
            Dead(other.ClosestPoint(transform.position), false);
        }
        if (other.CompareTag("Obstacle_noink"))
        {
            Dead(other.ClosestPoint(transform.position), true);
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

    void OnTriggerEnterGoal(Collider other)
    {
        var goal = other.gameObject.GetComponent<GoalController>();
        if (goal == null) return;
        // if (Variables.screenState != ScreenState.Game) return;
        // Variables.screenState = ScreenState.Clear;
        // speedZ = 0;
        // characterManager.Dance();
        // cameraController.IsFollow = false;
        characterManager.playerState = PlayerState.GoalBonus;
        cameraController.CameraState = CameraState.ClimbingStairs;
        speedZ *= 1.5f;
    }

    void OnTriggerEnterGoalStair(Collider other)
    {
        var goalStair = other.gameObject.GetComponent<GoalStairController>();
        if (goalStair == null) return;
        Leave();
    }

    public void Dead(Vector3 hitPos, bool isHitGate)
    {
        if (SoundManager.i)
        {
            if (!SoundManager.i.IsPlaying) SoundManager.i.PlayOneShot(2);
        }

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
        hitPos.y += Height / 2f;
        inkSr.transform.position = hitPos;
        inkSr.transform.DOScale(inkScale, 0.5f);
    }

    void Leave()
    {
        characterManager.Characters.Remove(this);
        rb.isKinematic = true;
        animator.SetBool("IsFall", false);
        animator.SetBool("IsRun", false);

        if (characterManager.ActiveCount > 0) return;
        animator.SetBool("IsDance", true);
        transform.forward = Vector3.back;
        cameraController.CameraState = CameraState.Rotate;
        Variables.screenState = ScreenState.Clear;

        Variables.goalRate = 1.0f;

        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 5f)) return;
        //        Debug.Log(hit.collider.gameObject.name);
        var goalStair = hit.collider.gameObject.GetComponent<GoalStairController>();
        if (goalStair == null) return;
        goalStair.Selected();
    }


}
