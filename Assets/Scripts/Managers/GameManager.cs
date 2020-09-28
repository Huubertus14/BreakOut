using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingetonMonobehaviour<GameManager>
{
    

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += MainMenuSceneLoaded;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
        SceneManager.sceneLoaded += GameSceneLoaded;
    }

    private void MainMenuSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
       //Do Stuff
    }

    private void GameSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //Do Stuff
    }


}
