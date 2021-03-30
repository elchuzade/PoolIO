using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayStatus : MonoBehaviour
{
    List<GameObject> allPlayers = new List<GameObject>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddPlayerScore(int hash)
    {
        if (hash != 0)
        {
            for (int i = 0; i < allPlayers.Count; i++)
            {
                if (allPlayers[i].GetHashCode() == hash)
                {
                    allPlayers[i].GetComponent<Ball>().AddScore();
                }
            }
        }
    }
}
