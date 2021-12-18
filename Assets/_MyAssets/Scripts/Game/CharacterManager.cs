using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using Zenject;

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
    [Inject] CameraController cameraController;
    public Vector3 BottomCharacterPos
    {
        get
        {
            if (pool.activelist.Count > 0)
            {
                bottomCharacterPos = pool.activelist[0].transform.position;
            }

            return bottomCharacterPos;
        }
    }
    Vector3 bottomCharacterPos;
    public int ActiveCount => activeCount;
    int activeCount;
    float deltaX;
    public PlayerState playerState { get; set; } = PlayerState.BeforeStart;
    public ObjectPool pool;

    public void OnAwake()
    {
        dummyGo.SetActive(false);
        pool.CreateInstance(characterPrefab, 100, (character) =>
        {
            character.OnInstantiate(this);
        });
        // Application.targetFrameRate = 300;
    }

    public void OnStart()
    {
        pool.ActivateReserves(1, out List<Character> additionalCharacters, (character) =>
        {
            character.OnInstantiate(this);
        });
        pool.activelist[0].Appear(transform.position, transform.position, 0, false);
        cameraController.SetOffset(pool.activelist[0].transform.position);

        this.ObserveEveryValueChanged(_ => pool.activelist.Count)
            .Subscribe(_ =>
            {
                activeCount = _;
                if (_ > 0) return;
                if (Variables.screenState != ScreenState.Game) return;
                if (playerState != PlayerState.Playing) return;
                Variables.screenState = ScreenState.Failed;
                playerState = PlayerState.AfterFinishedGame;
            });

        this.ObserveEveryValueChanged(_ => SaveData.i.startHumanCount)
        .Subscribe(_ => OnChangedStartHumanCount(_));
    }

    void OnChangedStartHumanCount(int startHumanCount)
    {
        AppearToStack(startHumanCount - pool.activelist.Count, 0, false);
    }


    public void OnUpdate()
    {
        deltaX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime / Time.deltaTime * (float)Screen.width / 750f;
        switch (playerState)
        {
            case PlayerState.BeforeStart:
                Stop();
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

        for (int i = 0; i < pool.activelist.Count; i++)
        {
            pool.activelist[i].Move(deltaX, i);
        }
    }

    void GoalBonus()
    {
        for (int i = 0; i < pool.activelist.Count; i++)
        {
            pool.activelist[i].Stair(i);
        }
    }

    void Stop()
    {
        for (int i = 0; i < pool.activelist.Count; i++)
        {
            pool.activelist[i].Stop();
        }
    }

    public void Dance()
    {
        playerState = PlayerState.AfterFinishedGame;
        for (int i = 0; i < pool.activelist.Count; i++)
        {
            pool.activelist[i].Dance();
        }
    }

    public void AppearToStack(int addCount, float addDelay, bool isOnSound)
    {
        Character topCharacter = pool.activelist[pool.activelist.Count - 1];

        pool.ActivateReserves(addCount, out List<Character> additionalCharacters, (character) =>
        {
            character.OnInstantiate(this);
        });

        Vector3 pos = topCharacter ? topCharacter.transform.position : BottomCharacterPos;

        float delay = 0f;
        for (int i = 0; i < additionalCharacters.Count; i++)
        {
            pos.y += topCharacter.Height;
            delay += addDelay;
            additionalCharacters[i].Appear(BottomCharacterPos, pos, delay, isOnSound);
        }
    }



    public void Dead(int deadCount)
    {
        bool isKillTop = false;
        var activeCharacters = pool.activelist;
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
