using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayStatus : MonoBehaviour
{
    [SerializeField] GameObject players;
    [SerializeField] GameObject playerCanvas;
    [SerializeField] GameObject playerCanvases;

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




    // When any player scores update that player's score


    // When any player connects refresh all players'names
    public void AddPlayer(GameObject player)
    {
        GameObject canvas = Instantiate(playerCanvas, player.transform.position, Quaternion.identity);
        canvas.transform.Find("Name").GetComponent<Text>().text = player.GetComponent<Ball>().GetNickname();
        canvas.transform.Find("Score").GetComponent<Text>().text = player.GetComponent<Ball>().GetScore().ToString();

        canvas.transform.SetParent(playerCanvases.transform);

        for (int i = 0; i < players.transform.childCount; i++)
        {
            Debug.Log(players.transform.GetChild(i).GetComponent<Ball>().GetNickname());
            Debug.Log(players.transform.GetChild(i).transform.position);
        }
    }

    // When any player disconnects refresh all players'names
    public void RemovePlayer()
    {
        for (int i = 0; i < players.transform.childCount; i++)
        {
            Debug.Log(players.transform.GetChild(i).GetComponent<Ball>().GetNickname());
            Debug.Log(players.transform.GetChild(i).transform.position);
        }
    }
}
