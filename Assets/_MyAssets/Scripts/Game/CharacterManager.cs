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

    void Awake()
    {
        dummyGo.SetActive(false);
        InstantiateCharacters(100);
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
        Characters[0].Appear(transform.position);

        this.ObserveEveryValueChanged(_ => Characters.Count(_ => _.gameObject.activeSelf))
            .Subscribe(_ => activeCount = _)
            .AddTo(this.gameObject);
    }


    void Update()
    {
        float deltaX;

        if (Input.GetMouseButton(0))
        {
            deltaX = Input.GetAxis("Mouse X") * 5f;
        }
        else
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
        for (int i = 0; i < additionalCharacters.Length; i++)
        {
            pos.y += topCharacter.Height;
            additionalCharacters[i].Appear(pos);
        }
    }
}
