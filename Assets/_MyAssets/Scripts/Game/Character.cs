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
    bool isMovingAppear;


    /// <summary>
    /// startより先
    /// </summary>
    /// <param name="characterManager"></param>
    public void OnInstantiate(CharacterManager characterManager)
    {
        bloodPsChildren = bloodPs.GetComponentsInChildren<ParticleSystem>();
        this.ObserveEveryValueChanged(_ => SaveData.i.selectedSkinIndex)
            .Subscribe(_ => OnChangedSkin(_));

        this.ObserveEveryValueChanged(_ => SaveData.i.selectedMaterialIndex)
            .Subscribe(_ => OnChangedMaterial(_));

        inkScale = inkSr.transform.lossyScale;
        inkSr.gameObject.SetActive(false);

        this.characterManager = characterManager;
        capsuleCollider.enabled = false;
        boxCollider.enabled = false;
    }

    /// <summary>
    /// OnInstantiateの次
    /// </summary>
    void Start()
    {
        // awake.OnInstantiate,プレハブに手動アタッチはだめだった
        gameObject.AddComponent<ZenAutoInjecter>();
    }


    void OnChangedSkin(int selectedSkinIndex)
    {
        SkinController skinController = Instantiate(SkinSettingSO.i.characterSkinDatas[selectedSkinIndex].prefab, transform);
        skinController.OnInstantiate();
        Destroy(animator.gameObject);
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




    public void Appear(Vector3 bottomPos, Vector3 targetPos, float duration, bool isOnSound)
    {
        isMovingAppear = true;
        rb.mass = 1f;
        capsuleCollider.enabled = false;
        boxCollider.enabled = false;
        transform.position = bottomPos;
        rb.DOMoveY(targetPos.y, duration)
        .OnComplete(() =>
        {
            isMovingAppear = false;
            capsuleCollider.enabled = true;
            boxCollider.enabled = true;
            if (isOnSound) SoundManager.i?.PlayOneShot(0);
        });
    }

    public void Move(float deltax, int index)
    {
        vel = rb.velocity;
        vel.z = speedZ;
        vel.x = Mathf.SmoothDamp(rb.velocity.x, deltax * Variables.speedX, ref currentVelocity, Variables.smoothTimeX);
        if (float.IsNaN(vel.x)) vel.x = 0;
        rb.velocity = vel;

        rb.mass = characterManager.pool.activelist.Count - index;

        if (characterManager.pool.activelist[0] == this)
        {
            animator.SetBool("IsRun", true);
            rb.AddForce(Vector3.down * 30f, ForceMode.Acceleration);
        }
        else
        {
            animator.SetBool("IsFall", true);
            PosCorrect();

            float distance = rb.position.y - characterManager.pool.activelist[index - 1].rb.position.y;
            if (distance / Height < 1.1f) return;
            rb.AddForce(Vector3.down * 30f, ForceMode.Acceleration);
        }

    }

    void PosCorrect()
    {
        // ズレ矯正
        var bottomXZ = characterManager.pool.activelist[0].rb.position;
        bottomXZ.y = 0;
        var thisXZ = rb.position;
        thisXZ.y = 0;
        float distance = Vector3.Distance(bottomXZ, thisXZ);
        if (distance > 0.1f)
        {
            thisXZ = bottomXZ;
            thisXZ.y = rb.position.y;
            rb.position = thisXZ;
        }
    }

    public void Stair(int index)
    {
        var vel = rb.velocity;
        vel.x = -transform.position.x * Variables.speedX;
        vel.z = speedZ * 2.0f;
        vel.y = 0;
        rb.velocity = vel;
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
            characterManager.AppearToStack(addCount, 0.05f, true);
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
        characterManager.pool.Remove(this);

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

    bool isLeft;
    void Leave()
    {
        // 1フレームに複数回判定するため
        if (isLeft) return;
        isLeft = true;

        // 階段で止まったときに例外的にアクティブにしたいから
        characterManager.pool.activelist.Remove(this);
        rb.isKinematic = true;
        animator.SetBool("IsFall", false);
        animator.SetBool("IsRun", false);

        // removeしてもこのタイミングでは数は減らない
        if (characterManager.ActiveCount > 1) return;
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
