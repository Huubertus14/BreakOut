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
    }

    public void AddLive(int lives = 1)
    {

    }

    public void RemoveLive(int lives = 1)
    {
        for (int i = 0; i < lives; i++)
        {
            hearts.RemoveAt(hearts.Count - 1);
        }
    }
}
