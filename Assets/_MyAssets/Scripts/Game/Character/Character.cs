using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
using UniRx;

public enum CharacterState
{
    Alive,
    Dead,
    Goaled,
}

public class Character : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] Animator animator;
    [SerializeField] IncEffectController inkEffectController;
    [SerializeField] ParticleSystem appearPs;
    [SerializeField] RagdollController ragdollController;
    [Inject] CameraController cameraController;
    float speedZ = 15f * 2f / 3f;
    float currentVelocity;
    CharacterManager characterManager;
    public float Height => capsuleCollider.height;
    bool isMovingAppear;
    SkinController skinController;
    CharacterState characterState = CharacterState.Alive;
    float lastFramePosX;
    float fallingTime;

    /// <summary>
    /// startより先
    /// </summary>
    /// <param name="characterManager"></param>
    public void OnInstantiate(CharacterManager characterManager)
    {
        inkEffectController.OnInstantiate();

        this.ObserveEveryValueChanged(_ => SaveData.i.selectedSkinIndex)
            .Subscribe(_ => OnChangedSkin(_));

        this.characterManager = characterManager;
        capsuleCollider.enabled = false;
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
        skinController = Instantiate(SkinSettingSO.i.CharacterSkinDatas[selectedSkinIndex].prefab, transform);
        skinController.OnInstantiate();
        Destroy(animator.gameObject);
        animator = skinController.Animator;
        ragdollController.SetRagdoll(animator);
        ragdollController.EnableRagdoll(false);
        rb.isKinematic = true;
        if (Variables.isSkinReal) animator.transform.localScale = Vector3.one * 1.2f;
    }

    public void Appear(Vector3 bottomPos, Vector3 targetPos, float duration, bool isOnSound)
    {
        characterState = CharacterState.Alive;
        ragdollController.EnableRagdoll(false);
        isMovingAppear = true;
        rb.isKinematic = true;
        capsuleCollider.enabled = false;
        transform.position = bottomPos;
        transform.DOMoveY(targetPos.y, duration)
        .OnComplete(() =>
        {
            isMovingAppear = false;
            capsuleCollider.enabled = true;

            if (isOnSound)
            {
                SoundManager.i?.PlayOneShot(0);
                //   VibrateManager.Play();
                AddCountTextEffectManager.i.Show(1, transform);
                if (!Variables.isSkinReal) appearPs.Play();
            }
        });
    }

    public void Move(float deltax, int index)
    {
        //skinController.EnableMesh(index < 24);
        gameObject.SetActive(index < 24);

        float velX = transform.position.x - lastFramePosX;
        float speedX = Mathf.SmoothDamp(velX, deltax * Variables.speedX * Time.deltaTime, ref currentVelocity, Variables.smoothTimeX);

        transform.AddPosX(speedX);
        transform.AddPosZ(speedZ * Time.deltaTime);
        PosCorrect();
        if (transform.position.x < -3.5f) transform.SetPosX(-3.5f);
        if (3.5f < transform.position.x) transform.SetPosX(3.5f);
        lastFramePosX = transform.position.x;

        float underY = 0;

        if (index == 0)
        {
            animator.SetTrigger("Run");
            underY = GetFloorSurfaceY();
        }
        else
        {
            animator.SetTrigger("Fall");
            underY = characterManager.pool.activelist[index - 1].transform.position.y + Height;
        }

        if (isMovingAppear) return;

        Fall(underY);
    }

    float GetFloorSurfaceY()
    {
        float underY = 0;
        Ray ray = new Ray(transform.position + Vector3.up * Height, Vector3.down);
        var hits = Physics.SphereCastAll(ray.origin, capsuleCollider.radius, ray.direction, 20);
        foreach (var hit in hits)
        {
            if (!hit.collider.gameObject.CompareTag("Floor")) continue;
            underY = hit.point.y;
            break;
        }
        return underY;
    }

    void Fall(float groundY)
    {
        float distance = transform.position.y - groundY;
        if (distance > 0.1f)
        {
            float velocityY = Physics.gravity.y * fallingTime;
            transform.AddPosY(velocityY * Time.deltaTime);
            fallingTime += Time.deltaTime;
        }
        else
        {
            fallingTime = 0;
            transform.SetPosY(groundY);
        }
    }

    void PosCorrect()
    {
        // ズレ矯正
        var bottomXZ = characterManager.pool.activelist[0].transform.position;
        bottomXZ.y = 0;
        var thisXZ = transform.position;
        thisXZ.y = 0;
        float distance = Vector3.Distance(bottomXZ, thisXZ);
        if (distance > 0.1f)
        {
            thisXZ = bottomXZ;
            thisXZ.y = transform.position.y;
            transform.position = thisXZ;
        }
    }

    public void Stair(int index)
    {
        // skinController.EnableMesh(index < 24);
        gameObject.SetActive(index < 24);

        if (index == 0)
        {
            animator.SetTrigger("Run");
        }
        else
        {
            animator.SetTrigger("Fall");
        }

        var addPos = Vector3.zero;
        addPos.x = -transform.position.x * Variables.speedX * Time.deltaTime;
        addPos.z = speedZ * 2.0f * Time.deltaTime;
        transform.position += addPos;

        // PosCorrect();
    }

    public void Stop()
    {
        // rb.velocity = Vector3.zero;
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

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Obstacle"))
        {
            Dead(collisionInfo.contacts[0].point, false);
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
        if (GoalController.i.gameObject != other.gameObject) return;
        if (characterManager.playerState != PlayerState.Playing) return;
        // Variables.screenState = ScreenState.Clear;
        // speedZ = 0;
        // characterManager.Dance();
        // cameraController.IsFollow = false;
        characterManager.playerState = PlayerState.GoalBonus;
        cameraController.CameraState = CameraState.ClimbingStairs;

        characterManager.Goal();
    }

    void OnTriggerEnterGoalStair(Collider other)
    {
        var goalStair = other.gameObject.GetComponent<GoalStairController>();
        if (goalStair == null) return;
        Leave(goalStair);
        transform.SetPosZ(other.ClosestPoint(transform.position).z - capsuleCollider.radius);
    }

    /// <summary>
    /// 2回呼ばれてるので注意
    /// </summary>
    /// <param name="hitPos"></param>
    /// <param name="isHitGate"></param>
    public void Dead(Vector3 hitPos, bool isHitGate)
    {
        // 1フレームに複数回判定するため
        if (characterState == CharacterState.Dead) return;
        characterState = CharacterState.Dead;
        SoundManager.i?.PlayOneShotDead();

        characterManager.pool.Remove(this);
        inkEffectController.PlayBloodParticle(transform.position, Height);

        if (Variables.isSkinReal)
        {
            ragdollController.EnableRagdoll(true);
            ragdollController.Addforce(Vector3.right * Random.Range(-1f, 1f) * 10f, ForceMode.Impulse);
            DOVirtual.DelayedCall(1.5f, () =>
            {
                if (characterState != CharacterState.Dead) return;
                gameObject.SetActive(false);
            });
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (isHitGate) return;
        inkEffectController.ShowInkSprite(transform.position, Height, capsuleCollider.radius);
    }

    void Leave(GoalStairController goalStairController)
    {
        // 1フレームに複数回判定するため
        if (characterState == CharacterState.Goaled) return;
        characterState = CharacterState.Goaled;

        // 階段で止まったときに例外的にアクティブにしたいから
        characterManager.pool.Remove(this);
        // rb.isKinematic = true;
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Fall");
        animator.SetTrigger("Idle");

        if (goalStairController.isLast)
        {
            GoalLastCharacter();
            return;
        }
        else
        {
            goalStairController.Passed();

            // 処理負荷
            if (characterManager.pool.activelist.Count > 3)
            {
                DOVirtual.DelayedCall(1.0f, () => gameObject.SetActive(false));
            }
        }

        if (characterManager.ActiveCount > 0) return;
        GoalLastCharacter();
    }

    void GoalLastCharacter()
    {

        animator.SetTrigger("Dance");
        transform.forward = Vector3.back;
        cameraController.CameraState = CameraState.Rotate;
        Variables.screenState = ScreenState.Clear;
        characterManager.playerState = PlayerState.AfterFinishedGame;

        Variables.goalRate = 1.0f;

        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 5f)) return;
        //        Debug.Log(hit.collider.gameObject.name);
        var goalStair = hit.collider.gameObject.GetComponent<GoalStairController>();
        if (goalStair == null) return;
        goalStair.Selected();
    }


}
