using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] Character characterPrefab;
    [SerializeField] GameObject dummyGo;
    public List<Character> Characters { get; set; } = new List<Character>();
    public Vector3 BottomCharacterPos => Characters[0].transform.position;
    public int ActiveCount => activeCount;
    int activeCount;
    bool isStart;
    float deltaX;

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
                Variables.screenState = ScreenState.Failed;
            })
            .AddTo(this.gameObject);

        deltaX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime / Time.deltaTime;
    }


    void Update()
    {
        deltaX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime / Time.deltaTime * (float)Screen.width / 750f;

        if (Input.GetMouseButtonDown(0))
        {
            isStart = true;
        }

        if (!isStart) return;

        if (!Input.GetMouseButton(0))
        {
            deltaX = 0;
        }

        if (Variables.screenState != ScreenState.Game)
        {
            deltaX = 0;
        }

        if (Characters.Count == 0) return;
        Characters[0].VelocityControl(deltaX);
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
        var activeCharacters = Characters.Where(_ => _.gameObject.activeSelf).ToArray();
        int lackCount = deadCount - activeCount;
        if (lackCount > 0)
        {
            deadCount = activeCount;
        }

        for (int i = 0; i < deadCount; i++)
        {
            activeCharacters[i].Dead(Vector3.zero, true);
        }
    }
}
