using UnityEngine;

public class CharacterPick : MonoBehaviour
{
    CharacterSelectManager characterSelectManager;
    LevelManager levelManager;

    void Awake()
    {
        characterSelectManager = FindFirstObjectByType<CharacterSelectManager>();
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    public void SelectBronzy()
    {
        characterSelectManager.ChooseBronzy();
        levelManager.LoadFirstLevel();
    }

    public void SelectReddy()
    {
        characterSelectManager.ChooseReddy();
        levelManager.LoadFirstLevel();
    }
}
