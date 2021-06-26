using System.Linq;
using GameModule.Class.Component.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GameModule.Class
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        
        public RoomIdInputField RoomIdInputField;

        [Tooltip("The UI Panel to let the user enter name, connect and play")] [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")] [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip(
            "The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        #endregion

        #region Private Fields
        private readonly string _gameVersion = "1";
        private bool _isConnecting;
        // private LoadBalancingClient _loadBalancingClient;
        private int _roomIdLength = 5;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (_isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                _isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}",
                cause);
            _isConnecting = false;
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");


            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");


                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("MazeRoom");
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"returnCode::{returnCode}, message::{message}");
            Debug.Log("JoinRoomFailed, going to try random room");
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(
                "PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            _JoinOrCreatePrivateRoom(_CreateRandomRoomId());
            // PhotonNetwork.CreateRoom(_CreateRandomRoomId(), new RoomOptions {MaxPlayers = maxPlayersPerRoom});
        }


        #endregion
        #region Public Methods

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                if (string.IsNullOrEmpty(RoomIdInputField.GetRoomId()))
                {
                    _JoinOrCreatePrivateRoom(_CreateRandomRoomId());
                }
                else
                {
                    _JoinOrCreatePrivateRoom(RoomIdInputField.GetRoomId());
                }
            }
            else
            {
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }
        }
        #endregion

        #region Private Methods
        
        private void _JoinOrCreatePrivateRoom(string roomId)
        {
            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            roomOptions.MaxPlayers = maxPlayersPerRoom;

            var enterRoomParams = new EnterRoomParams();
            enterRoomParams.RoomName = roomId;
            enterRoomParams.RoomOptions = roomOptions;
            PhotonNetwork.JoinOrCreateRoom(roomId, roomOptions, TypedLobby.Default);
            // _loadBalancingClient.OpJoinOrCreateRoom(enterRoomParams);
        }
        private string _CreateRandomRoomId()
        {
            var random = new System.Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, _roomIdLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }        

        #endregion
    }
}