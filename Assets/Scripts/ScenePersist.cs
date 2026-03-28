using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {

        // Singleton Pattern => Ensure only one ScenePersist instance can exist
        int numberScenePersists = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (numberScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        // End of singleton pattern
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
