using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainStatus : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField nameInput;

    [SerializeField] GameObject collectButton;
    [SerializeField] GameObject normalButton;
    [SerializeField] GameObject huntButton;

    string mode; // "Collect" "Normal" "Hunt" 0, 1, 2
    int modeIndex;

    void Start()
    {
        OnSwitchMode("Normal");
    }

    public void OnSwitchMode(string _mode)
    {
        if (_mode == "Collect")
        {
            collectButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            normalButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            huntButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            mode = "Collect";
            modeIndex = 0;
        } else if (_mode == "Normal")
        {
            collectButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            normalButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            huntButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            mode = "Normal";
            modeIndex = 1;
        } else if (_mode == "Hunt")
        {
            collectButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            normalButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            huntButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            mode = "Hunt";
            modeIndex = 2;
        }
    }

    public void OnPlayButtonClicked()
    {
        string playerName = nameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("Playername is invalid!");
        }
    }

    #region Photon Callbacks
    public void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " connected to server");
        // First connect to master then join or create a room
        ExitGames.Client.Photon.Hashtable customPropreties = new ExitGames.Client.Photon.Hashtable();
        customPropreties["md"] = modeIndex;

        PhotonNetwork.JoinRandomRoom(customPropreties, 0);
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    //public void JoinRandomRoom()
    //{
    //    ExitGames.Client.Photon.Hashtable customPropreties = new ExitGames.Client.Photon.Hashtable();
    //    customPropreties["md"] = modeIndex;

    //    PhotonNetwork.JoinRandomRoom(customPropreties, 0);
    //}

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("GamePlay");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    #endregion

    void CreateAndJoinRoom()
    {
        ExitGames.Client.Photon.Hashtable customPropreties = new ExitGames.Client.Photon.Hashtable();
        customPropreties["md"] = modeIndex;

        RoomOptions roomOptions = new RoomOptions() {
            CustomRoomProperties = customPropreties,
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 20,
            PublishUserId = true
        };

        roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
             "md",
        };

        string randomRoomName = "Room " + Random.Range(0, 10000);

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
}
