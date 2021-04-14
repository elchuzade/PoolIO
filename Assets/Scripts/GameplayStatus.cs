using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameplayStatus : MonoBehaviourPunCallbacks
{
    public class PlayerInfo
    {
        public string id;
        public string name;
        public int score;
    }

    [SerializeField] GameObject leaderboardPlayer1;
    [SerializeField] GameObject leaderboardPlayer2;
    [SerializeField] GameObject leaderboardPlayer3;
    [SerializeField] GameObject leaderboardPlayer4;
    [SerializeField] GameObject leaderboardPlayer5;

    List<PlayerInfo> allPlayers = new List<PlayerInfo>();

    List<int> removeIndexes = new List<int>();

    void Start()
    {
        GameObject playerBall = PhotonNetwork.Instantiate("PlayerBall", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //GameObject normalBot = PhotonNetwork.Instantiate("NormalBot", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //normalBot.GetComponent<NormalBot>().SetNameAndUserId("Cecil");
        //GameObject normalBot1 = PhotonNetwork.Instantiate("NormalBot", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //normalBot1.GetComponent<NormalBot>().SetNameAndUserId("Bob");
        //GameObject normalBot2 = PhotonNetwork.Instantiate("NormalBot", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //normalBot2.GetComponent<NormalBot>().SetNameAndUserId("Cecil");
        //GameObject normalBot3 = PhotonNetwork.Instantiate("NormalBot", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //normalBot3.GetComponent<NormalBot>().SetNameAndUserId("Elliot");
        //GameObject normalBot4 = PhotonNetwork.Instantiate("NormalBot", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //normalBot4.GetComponent<NormalBot>().SetNameAndUserId("Brett");
        //GameObject normalBot5 = PhotonNetwork.Instantiate("NormalBot", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //normalBot5.GetComponent<NormalBot>().SetNameAndUserId("Arnold");
        //GameObject normalBot6 = PhotonNetwork.Instantiate("NormalBot", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //normalBot6.GetComponent<NormalBot>().SetNameAndUserId("Finn");
        //GameObject normalBot7 = PhotonNetwork.Instantiate("NormalBot", transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
        //normalBot7.GetComponent<NormalBot>().SetNameAndUserId("Cecil");
    }

    public void AddPlayer(string _id, string _name, int _score)
    {
        PlayerInfo newPlayer = new PlayerInfo();
        newPlayer.id = _id;
        newPlayer.name = _name;
        newPlayer.score = _score;

        foreach (PlayerInfo player in allPlayers)
        {
            if (player.id == newPlayer.id)
            {
                return;
            }
        }

        allPlayers.Add(newPlayer);
        UpdateLeaderboard();
    }

    public void RemovePlayer(string _id)
    {
        foreach (PlayerInfo player in allPlayers)
        {
            if (player.id == _id)
            {
                allPlayers.Remove(player);
                break;
            }
        }

        UpdateLeaderboard();
    }

    public void AddScore(string id, int score)
    {
        foreach (PlayerInfo player in allPlayers)
        {
            if (player.id == id)
            {
                player.score += score;
                break;
            }
        }

        BallInfo[] allBallInfos = FindObjectsOfType<BallInfo>();

        for (int i = 0; i < allBallInfos.Length; i++)
        {
            if (allBallInfos[i].userId == id)
            {
                allBallInfos[i].gameObject.GetComponent<Ball>().IncreaseScore(score);
            }
        }

        UpdateLeaderboard();
    }

    public void UpdateLeaderboard()
    {
        removeIndexes.Clear();

        for (int i = 0; i < allPlayers.Count; i++)
        {
            bool exists = false;
            for (int j = 0; j < PhotonNetwork.PlayerList.Length; j++)
            {
                if (PhotonNetwork.PlayerList[j].UserId == allPlayers[i].id)
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                removeIndexes.Add(i);
            }
            exists = false;
        }

        for (int i = 0; i < removeIndexes.Count; i++)
        {
            allPlayers.Remove(allPlayers[removeIndexes[i]]);
        }

        allPlayers.Sort((x, y) => y.score.CompareTo(x.score));
        if (allPlayers.Count > 0)
        {
            leaderboardPlayer1.GetComponent<Text>().text = allPlayers[0].score + " - " + allPlayers[0].name;
            leaderboardPlayer2.GetComponent<Text>().text = "Empty";
            leaderboardPlayer3.GetComponent<Text>().text = "Empty";
            leaderboardPlayer4.GetComponent<Text>().text = "Empty";
            leaderboardPlayer5.GetComponent<Text>().text = "Empty";
        }
        if (allPlayers.Count > 1)
        {
            leaderboardPlayer2.GetComponent<Text>().text = allPlayers[1].score + " - " + allPlayers[1].name;
            leaderboardPlayer3.GetComponent<Text>().text = "Empty";
            leaderboardPlayer4.GetComponent<Text>().text = "Empty";
            leaderboardPlayer5.GetComponent<Text>().text = "Empty";
        }
        if (allPlayers.Count > 2)
        {
            leaderboardPlayer3.GetComponent<Text>().text = allPlayers[2].score + " - " + allPlayers[2].name;
            leaderboardPlayer4.GetComponent<Text>().text = "Empty";
            leaderboardPlayer5.GetComponent<Text>().text = "Empty";
        }
        if (allPlayers.Count > 3)
        {
            leaderboardPlayer4.GetComponent<Text>().text = allPlayers[3].score + " - " + allPlayers[3].name;
            leaderboardPlayer5.GetComponent<Text>().text = "Empty";
        }
        if (allPlayers.Count > 4)
        {
            leaderboardPlayer5.GetComponent<Text>().text = allPlayers[4].score + " - " + allPlayers[4].name;
        }
    }
}
