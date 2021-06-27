using System;
using System.Collections.Generic;
using System.Linq;
using GameModule.Class.Component.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class
{
    public class Launcher : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        [SerializeField] private RoomIdInputField _roomIdInputField;
        [SerializeField] private GameObject _controlPanel;

        [SerializeField] private GameObject _progressLabel;
        [SerializeField] private GameObject _waitingPanel;
        [SerializeField] private GameObject _startButton;
        [SerializeField] private List<Text> _playerNameList;
        #endregion

        #region Private Serializable Fields

        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        #endregion

        #region Private Fields

        private readonly string _gameVersion = "1";
        private bool _isConnecting;
        private int _roomIdLength = 5;
        private Player[] _playerList;
        private int _localPlayerIndex; 
        private bool _isGoodCitizenChosen;
        private string _goodCitizenUserId;
        
        private bool _isHackerChosen;
        private string _hackerUserId;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            _controlPanel.SetActive(true);
            _progressLabel.SetActive(false);
            _waitingPanel.SetActive(false);
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            if (_isConnecting)
            {
                _CreateRoomBasedOnInputField();
                _isConnecting = false;
            }
        }
        public override void OnJoinedRoom()
        {
            _controlPanel.SetActive(false);
            _progressLabel.SetActive(false);
            _waitingPanel.SetActive(true);
            _UpdatePlayerList();
        }
        
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName); // not seen if you're the player connecting
            _UpdatePlayerList();
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName); // seen when other disconnects

            _UpdatePlayerList();
            if (PhotonNetwork.IsMasterClient)
            {

            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
            _isConnecting = false;
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"returnCode::{returnCode}, message::{message}");
            _JoinOrCreatePrivateRoom(_CreateRandomRoomId());
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            _JoinOrCreatePrivateRoom(_CreateRandomRoomId());
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _JoinOrCreatePrivateRoom(_CreateRandomRoomId());
        }

        #endregion
        #region Public Methods

        public void ConnectButtonClicked()
        {
            _controlPanel.SetActive(false);
            _progressLabel.SetActive(true);
            
            if (PhotonNetwork.IsConnected)
            {
                _CreateRoomBasedOnInputField();
            }
            else
            {
                _isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }
        }

        public void StartGameButtonClicked()
        {
            if (string.IsNullOrEmpty(_hackerUserId) && string.IsNullOrEmpty(_goodCitizenUserId)) return;
            
            for (var index = 0; index < _playerList.Length; index++)
            {
                var player = _playerList[index];
                if (_goodCitizenUserId == player.NickName)
                {
                    player.JoinTeam("GoodCitizen");
                }
                if (_hackerUserId == player.NickName)
                {
                    player.JoinTeam("Hacker");
                }
            }

            PhotonNetwork.LoadLevel("MazeRoom");
        }

        public void OnGoodCitizenButtonClicked()
        {
            _isGoodCitizenChosen = !_isGoodCitizenChosen;
            photonView.RPC("_RPC_SendGoodCitizenButtonClicked", RpcTarget.AllBuffered, _isGoodCitizenChosen ? PhotonNetwork.LocalPlayer.NickName : string.Empty);
        }

        [PunRPC]
        private void _RPC_SendGoodCitizenButtonClicked(string name)
        {
            _goodCitizenUserId = name;
            Debug.Log($"_RPC_SendGoodCitizenButtonClicked:: {_goodCitizenUserId}");
        }
        public void OnHackerButtonClicked()
        {
            _isHackerChosen = !_isHackerChosen;
            photonView.RPC("_RPC_SendHackerButtonClicked", RpcTarget.AllBuffered, _isHackerChosen ? PhotonNetwork.LocalPlayer.NickName : string.Empty);
        }
        [PunRPC]
        private void _RPC_SendHackerButtonClicked(string name)
        {
            _hackerUserId = name;
            Debug.Log($"_RPC_SendHackerButtonClicked:: {_hackerUserId}");

        }
        private void _CreateRoomBasedOnInputField()
        {
            if (string.IsNullOrEmpty(_roomIdInputField.GetRoomId()))
            {
                _JoinOrCreatePrivateRoom(_CreateRandomRoomId());
            }
            else
            {
                _JoinOrCreatePrivateRoom(_roomIdInputField.GetRoomId());
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
        }
        private string _CreateRandomRoomId()
        {
            var random = new System.Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, _roomIdLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }        
        private void _UpdatePlayerList()
        {
            _playerList = PhotonNetwork.PlayerList;
            for (var index = 0; index < _playerList.Length; index++)
            {
                var player = _playerList[index];
                if (player.IsLocal) 
                    _localPlayerIndex = index;
                _playerNameList[index].text = player.NickName;
            }

            _startButton.SetActive(_playerList.Length == maxPlayersPerRoom);
        }

        #endregion
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // if (stream.IsWriting)
            // {
            //     stream.SendNext(_isGoodCitizenChosen);
            //     stream.SendNext(_isHackerChosen);
            // }
            // else
            // {
            //     _isGoodCitizenChosen = (bool) stream.ReceiveNext();
            //     _isHackerChosen = (bool) stream.ReceiveNext();
            // }
        }

    }
}