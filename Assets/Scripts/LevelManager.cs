using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadCharacterSelectionScene()
    {
        SceneManager.LoadScene("Char Select");
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
