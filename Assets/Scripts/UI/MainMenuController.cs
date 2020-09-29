using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainMenuController : MonoBehaviour
{
    [Header("Refs:")]
    [SerializeField] private TextMeshProUGUI highScoreText;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.SP.GetPlayerData != null);
        SetMainMenuText();
    }

    public void StartGame()
    {
        GameManager.SP.StartGame();
    }

    public void SetMainMenuText()
    {
        highScoreText.text = "HighScore: " + GameManager.SP.GetPlayerData.playerHighScore;
    }
}
