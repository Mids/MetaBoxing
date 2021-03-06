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

            if (PhotonNetwork.IsConnected)
                PhotonNetwork.JoinLobby();
            else
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

// #if UNITY_EDITOR
            if(!PlayerPrefs.HasKey("IS3PP"))
                PlayerPrefs.SetInt("IS3PP", 1);
            if(!PlayerPrefs.HasKey("ISPHYSICS"))
                PlayerPrefs.SetInt("ISPHYSICS", 1);
// #endif

            if (PlayerPrefs.GetInt("ISPHYSICS") == 1)
                PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation);
            else
                PhotonNetwork.Instantiate("KinematicPlayer", points[idx].position, points[idx].rotation);

            var offset = Vector3.zero;

            if (PlayerPrefs.GetInt("IS3PP") == 1)
                offset = points[idx].rotation * new Vector3(0, 0.5f, -1);

            var xrRig = GameObject.Find("XR Rig").GetComponent<XRRig>();
            xrRig.MoveCameraToWorldLocation(points[idx].position + new Vector3(0, 0.8f, 0) + offset);
            xrRig.MatchRigUpCameraForward(Vector3.up, points[idx].forward);
        }

        public void OnDestroy()
        {
            if(PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();
            if(PhotonNetwork.InLobby)
                PhotonNetwork.LeaveLobby();
        }
    }
}