using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("PreLobbyScene");
    }

     public void HelpScene() 
    {
        SceneManager.LoadScene("Help");
    }

     public void OptionsScene() 
    {
        SceneManager.LoadScene("Options");
    }

     public void BackToMainMenu() 
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Exit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
