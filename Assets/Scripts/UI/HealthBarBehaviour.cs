using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is to visualize the player healthbar
/// </summary>
public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] private HearthBehaviour hearthPrefab;
    private HearthBehaviour[] hearts;

    private void Start()
    {
        StartCoroutine(CreateHealthBar());
    }

    public IEnumerator CreateHealthBar()
    {
        hearts = new HearthBehaviour[GameConstants.MAXLIVES];

        for (int i = 0; i < hearts.Length; i++)
        {
            HearthBehaviour _temp = Instantiate(hearthPrefab, transform.position, Quaternion.identity, transform);
            _temp.transform.SetSiblingIndex(i);
            hearts[i] = _temp;

            yield return 0;

            if (i > MatchManager.SP.GetMatchLives)
            {
                hearts[i].gameObject.SetActive(false);
            }
            else
            {
                hearts[i].EnableHearth();
            }
        }
    }

    public void UpdateLives()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i > MatchManager.SP.GetMatchLives)
            {
                hearts[i].gameObject.SetActive(false);
            }
            else
            {
                if (!hearts[i].gameObject.activeSelf)
                {
                    hearts[i].gameObject.SetActive(true);
                    hearts[i].EnableHearth();
                }
            }
        }
    }
}
