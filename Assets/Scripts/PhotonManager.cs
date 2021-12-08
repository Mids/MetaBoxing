using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MetaBoxing
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        private readonly string version = "1.0f";
        private readonly string userId = "KAIST";

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = version;
            PhotonNetwork.NickName = userId;

            print(PhotonNetwork.SendRate);

            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            print("Connected to Master");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            print("Joined Lobby");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            print($"JointRandomFailed {returnCode}: {message}");
            var ro = new RoomOptions
            {
                MaxPlayers = 20,
                IsOpen = true,
                IsVisible = true
            };

            PhotonNetwork.CreateRoom("My Room", ro);
        }

        public override void OnCreatedRoom()
        {
            print("Created Room");
            print($"Room Name: {PhotonNetwork.CurrentRoom.Name}");
        }

        public override void OnJoinedRoom()
        {
            print("Joined Room");
            print($"Player count = {PhotonNetwork.CurrentRoom.PlayerCount}");

            foreach (var player in PhotonNetwork.CurrentRoom.Players)
                print($"{player.Value.NickName}, {player.Value.ActorNumber}");

            var points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
            int idx = PhotonNetwork.CurrentRoom.PlayerCount;

            PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation);

            var offset = Vector3.zero;

            if (PlayerPrefs.GetInt("IS3PP") == 0)
                offset = new Vector3(0, 1, -1);

            var xrRig = GameObject.Find("XR Rig").GetComponent<XRRig>();
            xrRig.MoveCameraToWorldLocation(points[idx].position + new Vector3(0, 0.8f, 0) + offset);
            xrRig.MatchRigUpCameraForward(Vector3.up, points[idx].forward);
        }
    }
}