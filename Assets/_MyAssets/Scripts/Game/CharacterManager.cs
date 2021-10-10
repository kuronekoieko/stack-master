using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] Character characterPrefab;
    [SerializeField] GameObject dummyGo;
    public List<Character> Characters { get; set; } = new List<Character>();
    public Vector3 BottomCharacterPos => Characters[0].transform.position;

    void Awake()
    {
        dummyGo.SetActive(false);

        for (int i = 0; i < 100; i++)
        {
            var character = Instantiate(characterPrefab);
            Characters.Add(character);
            character.OnInstantiate();
        }

    }

    void Start()
    {
        Characters[0].Appear(transform.position);
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
        if (Characters.Count > 0) Characters[0].VelocityControl(deltaX);
    }
}
