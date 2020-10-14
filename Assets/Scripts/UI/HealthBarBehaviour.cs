using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to visualize the player healthbar
/// </summary>
public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject hearthPrefab;
    private List<GameObject> hearts;

    private void Start()
    {
        CreateHealthBar();
    }

    public void CreateHealthBar()
    {
        hearts = new List<GameObject>();

        for (int i = 0; i < GameConstants.MAXLIVES; i++)
        {
            GameObject _temp = Instantiate(hearthPrefab, transform.position, Quaternion.identity, transform);
            _temp.transform.SetSiblingIndex(i);
            hearts.Add(_temp);
        }
        UpdateLives();
    }

    public void UpdateLives()
    {
        for (int i = 0; i < GameConstants.MAXLIVES; i++) //disable all
        {
            hearts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < MatchManager.SP.GetMatchLives + 1; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }
}
