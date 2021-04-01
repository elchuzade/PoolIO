using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayStatus : MonoBehaviour
{
    [SerializeField] GameObject players;

    void Update()
    {
        PlaceNames();
    }

    private void PlaceNames()
    {
        for (int i = 0; i < players.transform.childCount; i++)
        {
            Debug.Log(players.transform.GetChild(i).GetComponent<Ball>().GetNickname());
            Debug.Log(players.transform.GetChild(i).transform.position);
        }
    }

    public void AddPlayerScore(int hash)
    {
        if (hash != 0)
        {
            for (int i = 0; i < players.transform.childCount; i++)
            {
                if (players.transform.GetChild(i).GetHashCode() == hash)
                {
                    players.transform.GetChild(i).GetComponent<Ball>().AddScore();
                }
            }
        }
    }

    // When any player disconnects or connects refresh all players'names
    // When any player scores update that player's score
}
