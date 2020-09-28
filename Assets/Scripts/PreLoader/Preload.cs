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
        Instantiate(persistancePrefab);
        DontDestroyOnLoad(persistancePrefab);

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
