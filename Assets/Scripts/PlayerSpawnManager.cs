using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    CharacterSelectManager characterSelectManager;

    void Awake()
    {
        characterSelectManager = FindFirstObjectByType<CharacterSelectManager>();
        InstantiateCharacter();
    }

    void InstantiateCharacter()
    {
        int currentCharacterIndex = characterSelectManager.GetCurrentCharacterIndex();
        
        for(int i = 0; i < characters.Length; i++)
        {
            if (i != currentCharacterIndex)
            {
                characters[i].SetActive(false);
                Destroy(characters[i]);
            }
        }
    }
}
