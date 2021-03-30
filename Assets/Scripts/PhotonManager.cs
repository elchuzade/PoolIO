using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerCamera;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 20;

        PhotonNetwork.JoinOrCreateRoom("RoomName", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GameObject ballStuff = PhotonNetwork.Instantiate("BallStuff", transform.position, Quaternion.identity);
        playerCamera.transform.SetParent(ballStuff.transform);
    }
}
