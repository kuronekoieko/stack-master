using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public enum PlayerState
{
    BeforeStart,
    Playing,
    GoalBonus,
    AfterFinishedGame,
}

public class CharacterManager : MonoBehaviour
{
    [SerializeField] Character characterPrefab;
    [SerializeField] GameObject dummyGo;
    public List<Character> Characters { get; set; } = new List<Character>();
    public Vector3 BottomCharacterPos
    {
        get
        {
            if (Characters[0].gameObject.activeSelf)
            {
                bottomCharacterPos = Characters[0].transform.position;
            }
            return bottomCharacterPos;
        }
    }
    Vector3 bottomCharacterPos;
    public int ActiveCount => activeCount;
    int activeCount;
    float deltaX;
    public PlayerState playerState { get; set; } = PlayerState.BeforeStart;

    void Awake()
    {
        dummyGo.SetActive(false);
        InstantiateCharacters(100);
        // Application.targetFrameRate = 300;
    }

    void InstantiateCharacters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var character = Instantiate(characterPrefab);
            Characters.Add(character);
            character.OnInstantiate(this);
        }
    }

    void Start()
    {
        Characters[0].Appear(transform.position, transform.position, 0f);

        this.ObserveEveryValueChanged(_ => Characters.Count(_ => _.gameObject.activeSelf))
            .Subscribe(_ =>
            {
                activeCount = _;
                if (_ > 0) return;
                if (Variables.screenState != ScreenState.Game) return;
                if (playerState != PlayerState.Playing) return;
                Variables.screenState = ScreenState.Failed;
                playerState = PlayerState.AfterFinishedGame;
            })
            .AddTo(this.gameObject);
    }


    void Update()
    {
        deltaX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime / Time.deltaTime * (float)Screen.width / 750f;
        switch (playerState)
        {
            case PlayerState.BeforeStart:

                if (Variables.isLaunchUIScene)
                {
                    if (Variables.screenState != ScreenState.Game) return;
                }
                else
                {
                    if (!Input.GetMouseButtonDown(0)) return;
                }

                playerState = PlayerState.Playing;

                break;
            case PlayerState.Playing: Run(); break;
            case PlayerState.GoalBonus: GoalBonus(); break;
            case PlayerState.AfterFinishedGame: Stop(); break;
            default: break;
        }

    }

    void Run()
    {
        if (!Input.GetMouseButton(0))
        {
            deltaX = 0;
        }

        if (Characters.Count == 0) return;
        Characters[0].VelocityControl(deltaX);
    }

    void GoalBonus()
    {
        Characters[0].VelocityControl(-Characters[0].transform.position.x);
    }

    void Stop()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].Stop();
        }
    }

    void FixedUpdate()
    {
        for (int i = 1; i < Characters.Count; i++)
        {
            Characters[i].Follow(Characters[0].transform.position);
        }
    }

    public void Dance()
    {
        playerState = PlayerState.AfterFinishedGame;
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].Dance();
        }
    }

    public void AppearToStack(int addCount)
    {
        int lackCount = addCount - Characters.Count(_ => !_.gameObject.activeSelf);
        if (lackCount > 0)
        {
            InstantiateCharacters(lackCount);
        }
        var additionalCharacters = Characters.Where(_ => !_.gameObject.activeSelf).Take(addCount).ToArray();
        Character topCharacter = Characters.Where(_ => _.gameObject.activeSelf).LastOrDefault();

        Vector3 pos = topCharacter.transform.position;
        float delay = 0f;
        for (int i = 0; i < additionalCharacters.Length; i++)
        {
            pos.y += topCharacter.Height;
            additionalCharacters[i].Appear(Characters[0].transform.position, pos, delay);
            delay += 0.05f;
        }
    }

    public void Dead(int deadCount)
    {
        bool isKillTop = false;
        var activeCharacters = Characters.Where(_ => _.gameObject.activeSelf).ToList();
        if (isKillTop)
        {
            activeCharacters = activeCharacters.Reverse<Character>().ToList();
        }

        var killedCharacters = activeCharacters.Take(deadCount).ToArray();

        for (int i = 0; i < killedCharacters.Length; i++)
        {
            killedCharacters[i].Dead(Vector3.zero, true);
        }
    }
}
