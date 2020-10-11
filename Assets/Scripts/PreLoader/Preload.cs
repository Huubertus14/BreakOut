using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour
{
    [SerializeField] private GameObject persistancePrefab;

    private void Awake()
    {
        GameObject tempPers = Instantiate(persistancePrefab);
        DontDestroyOnLoad(tempPers);

        StartCoroutine(StartLoadingGame());
    }


    private IEnumerator StartLoadingGame()
    {
        float timeOut = 25;

        //TODO Remove this when all i working again
        /*while (GameManager.SP.GetPlayerData == null)
        {
            GameManager.SP.SetPlayerData(SaveData.LoadData());
            Debug.Log("Load Data");
            yield return 0;
        }*/

        yield return 0;
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnBootMainMenuLoaded;
    }

    private void OnBootMainMenuLoaded(Scene _scene, LoadSceneMode arg1)
    {
        //Check if main menu

        //Load all data

        SceneManager.sceneLoaded -= OnBootMainMenuLoaded;
    }
}
