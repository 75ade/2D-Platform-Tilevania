using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    enum Character
    {
        Bronzy = 0,
        Reddy = 1
    }

    static CharacterSelectManager instance;
    int currentCharacterIndex;
    
    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetCurrentCharacterIndex()
    {
        return currentCharacterIndex;
    }

    public void ChooseBronzy()
    {
        currentCharacterIndex = (int)Character.Bronzy;
        Debug.Log("Choose Bronzy");
    }

    public void ChooseReddy()
    {
        currentCharacterIndex = (int)Character.Reddy;
        Debug.Log("Choose Reddy");
    }

    public void ResetCharacterIndex()
    {
        currentCharacterIndex = -1;
    }
}
