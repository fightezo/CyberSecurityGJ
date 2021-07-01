using System;
using System.Collections.Generic;
using System.Linq;
using ItemModule.Class.Data;
using MapModule.Class;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using PlayerModule.Class;
using PlayerModule.Class.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameModule.Class
{
    public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        public static GameManager Instance;

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        [SerializeField] private List<GameObject> _phasePanelList;
        [SerializeField] private List<Text> _playerNameList;
        [SerializeField] private GameObject _player0ButtonGroup;
        [SerializeField] private GameObject _player1ButtonGroup;

        [SerializeField] private GameObject _startButton;

        [SerializeField] private Text _roomIdText;
        [SerializeField] private Text _countDownText;
        [SerializeField] private Slider _securityLevelSlider;

        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private GameObject _gameOverPanel;

        #endregion

        #region Private Fields

        public enum GameState
        {
            WaitingForPlayers = 0,
            Planning = 1,
            Preparation = 2,
            Battle = 3,
            End = 4,
        }

        private Dictionary<GameState, float> TotalTimeList = new Dictionary<GameState, float>()
        {
            {GameState.WaitingForPlayers, 60f},
            {GameState.Planning, 10f},
            {GameState.Preparation, 30f},
            {GameState.Battle, 300f},
            {GameState.End, 120f},
        };

        private float _currentPlayTime = 0f;
        private GameState _currentState = GameState.WaitingForPlayers;

        [SerializeField] private int _securityLevel = 0;
        private int _citizenSecurityThreshold = 5;
        private int _citizenSecurityMax = 10;
        private int _hackerSecurityThreshold = 0;
        private int _hackerSecurityMax = -10;

        private PlayerManager _localPlayerManager;

        private Player[] _playerList;
        private int _localPlayerIndex;
        private List<Team> _playersTeamStateList = new List<Team>() {Team.None, Team.None};
        private string _localPlayerName;

        #endregion


        #region MonoBehaviour CallBacks

        private void Start()
        {
            Instance = this;
            _FirstTimeSetup();
            _ResetGameState();
            _UpdateViewPanel();
            _UpdatePlayerList();
        }

        private void Update()
        {
            if (_currentState != GameState.WaitingForPlayers && _currentState != GameState.End)
            {
                _currentPlayTime -= Time.deltaTime;
                var timeLeft = TimeSpan.FromSeconds(_currentPlayTime);
                _countDownText.text = $"{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
            }

            if (_currentPlayTime <= 0f)
            {
                _UpdateNextGameState();
            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}",
                newPlayer.NickName); // not seen if you're the player connecting
            _UpdatePlayerList();
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName); // seen when other disconnects

            // _UpdatePlayerList();
            // TODO: You Win~
            LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        #region Public Methods

        public GameState GetGameState()
        {
            return _currentState;
        }

        public void CheckState(PlayerManager self, PlayerManager taggedPlayer)
        {
            Debug.Log("Check State");
            // _localPlayerManager = self;
            if (_currentState == GameState.Battle)
            {
                _UpdateNextGameState();
            }
            else
            {
                Debug.LogWarning("Wrong Game State");
            }
        }

        public void LeaveRoom()
        {
            if (PhotonNetwork.NetworkClientState != ClientState.Leaving)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        
        #endregion

        #region Public Button Methods

        public void OnStartGameButtonClicked()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (_playersTeamStateList.Contains(Team.None)) return;
            _UpdateNextGameState();
        }

        public void OnCitizenButtonClicked()
        {
            if (_playersTeamStateList.Contains(Team.Citizen) &&
                _playersTeamStateList.FindIndex(x => x == Team.Citizen) != _localPlayerIndex)
                return;

            var newState = _playersTeamStateList[_localPlayerIndex];
            switch (newState)
            {
                case Team.None:
                case Team.Hacker:
                    newState = Team.Citizen;
                    break;
                case Team.Citizen:
                    newState = Team.None;
                    break;
            }

            var itemList = new[] {_localPlayerIndex, (int) newState};
            Debug.Log($"BEFORE::_RPC_SendCitizenButtonClicked:: {_playerList[itemList[0]]}::{newState}");
            photonView.RPC("_RPC_SendCitizenButtonClicked", RpcTarget.AllBuffered, itemList);
        }

        public void OnHackerButtonClicked()
        {
            if (_playersTeamStateList.Contains(Team.Hacker) &&
                _playersTeamStateList.FindIndex(x => x == Team.Hacker) != _localPlayerIndex)
                return;

            var newState = _playersTeamStateList[_localPlayerIndex];
            switch (newState)
            {
                case Team.None:
                case Team.Citizen:
                    newState = Team.Hacker;
                    break;
                case Team.Hacker:
                    newState = Team.None;
                    break;
            }

            var itemList = new[] {_localPlayerIndex, (int) newState};
            photonView.RPC("_RPC_SendHackerButtonClicked", RpcTarget.AllBuffered, itemList);
        }

        public void OnItemClicked()
        {
            
        }
        #endregion

        #region Private Methods

        private void _FirstTimeSetup()
        {
            _roomIdText.text = $"RoomID: {PhotonNetwork.CurrentRoom.Name}";
            _securityLevelSlider.minValue = _hackerSecurityMax;
            _securityLevelSlider.maxValue = _citizenSecurityMax;
            _securityLevelSlider.value = _securityLevel;
        }

        private void _UpdateViewPanel()
        {
            // var currentIndex = (_currentState > GameState.Planning && _currentState < GameState.End ) ? 2 : (int) _currentState;
            var currentIndex = (int) _currentState;
            Debug.Log($"panelList::{(GameState) currentIndex}");
            for (var index = 0; index < _phasePanelList.Count; index++)
            {
                _phasePanelList[index].SetActive(index == currentIndex);
            }
        }


        private void _UpdateNextGameState()
        {
            switch (_currentState)
            {
                case GameState.WaitingForPlayers:
                    _BeginPlanningPhase();
                    break;
                case GameState.Planning:
                    _BeginPreparationPhase();
                    break;
                case GameState.Preparation:
                    _BeginBattlePhase();
                    break;
                case GameState.Battle:
                    _BeginEndPhase();
                    break;
                case GameState.End:
                    _BeginAfterEnd();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            photonView.RPC("_RPC_SendUpdatedGameState", RpcTarget.Others, _currentState);
            _UpdateViewPanel();
        }


        private void _BeginPlanningPhase()
        {
            _currentState = GameState.Planning;
            _currentPlayTime = TotalTimeList[_currentState];
            photonView.RPC("_RPC_UpdatePlayerData", RpcTarget.AllBuffered);
        }

        private void _BeginPreparationPhase()
        {
            _currentState = GameState.Preparation;
            _currentPlayTime = TotalTimeList[_currentState];
        }

        private void _CreateLocalPlayerInstance()
        {
            if (playerPrefab != null)
            {
                var position = Vector3.zero;

                Debug.LogWarning($"BEFORE::PlayerManager.LocalPlayerInstance::{PlayerManager.LocalPlayerInstance}");
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    _localPlayerManager = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity, 0)
                        .GetComponent<PlayerManager>();
                }

                Debug.LogWarning($"AFTER::PlayerManager.LocalPlayerInstance::{PlayerManager.LocalPlayerInstance}");
            }
        }

        private void _BeginBattlePhase()
        {
            _currentState = GameState.Battle;
            _currentPlayTime = TotalTimeList[_currentState];
        }

        private void _BeginEndPhase()
        {
            _currentState = GameState.End;
            _currentPlayTime = TotalTimeList[_currentState];
            if (_securityLevel >= _citizenSecurityThreshold)
            {
                _victoryPanel.SetActive(_localPlayerManager.GetTeam() == Team.Citizen);
                _gameOverPanel.SetActive(_localPlayerManager.GetTeam() == Team.Hacker);
                //In the DarkWeb; capturing hacker
                // _DisplayCitizenEnding();
            }

            if (_securityLevel <= _hackerSecurityThreshold)
            {
                _victoryPanel.SetActive(_localPlayerManager.GetTeam() == Team.Hacker);
                _gameOverPanel.SetActive(_localPlayerManager.GetTeam() == Team.Citizen);
                // In Hacked Computer; steals identity
                // _DisplayHackerEnding();
            }

            _victoryPanel.SetActive(_localPlayerManager.GetState() == PlayerState.Invading);
            _gameOverPanel.SetActive(_localPlayerManager.GetState() != PlayerState.Invading);
        }

        private void _DisplayHackerEnding()
        {
        }

        private void _DisplayCitizenEnding()
        {
        }

        private void _BeginAfterEnd()
        {
        }

        private void _ResetGameState()
        {
            _currentState = GameState.WaitingForPlayers;
            _currentPlayTime = TotalTimeList[_currentState];
            _startButton.SetActive(PhotonNetwork.IsMasterClient);
        }


        private void _UpdatePlayerList()
        {
            _playerList = PhotonNetwork.PlayerList;
            for (var index = 0; index < _playerList.Length; index++)
            {
                var player = _playerList[index];
                if (player.IsLocal)
                {
                    _localPlayerIndex = index;
                    _localPlayerName = player.NickName;
                    _player0ButtonGroup.SetActive(index == 0);
                    _player1ButtonGroup.SetActive(index == 1);
                }

                _playerNameList[index].text = player.NickName;
            }

            _CreateLocalPlayerInstance();

            _startButton.SetActive(_playerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers);
        }

        #endregion

        #region PUNRPC Methods

        [PunRPC]
        private void _RPC_UpdatePlayerData()
        {
            var position = Vector3.zero;
            if (_playersTeamStateList[_localPlayerIndex] == Team.Citizen)
            {
                position = MapManager.Instance.GetCitizenSpawnPoint();
            }
            else if (_playersTeamStateList[_localPlayerIndex] == Team.Hacker)
            {
                position = MapManager.Instance.GetHackerSpawnPoint();
            }

            _localPlayerManager.UpdatePlayerData(position, _playersTeamStateList[_localPlayerIndex]);
        }

        [PunRPC]
        private void _RPC_SendUpdatedGameState(GameState gameState)
        {
            _currentState = gameState;
            _UpdateViewPanel();
        }

        [PunRPC]
        private void _RPC_SendCitizenButtonClicked(int[] newState)
        {
            _playersTeamStateList[newState[0]] = (Team) newState[1];
            Debug.Log(
                $"_RPC_SendCitizenButtonClicked:: {_playerList[newState[0]]}::{_playersTeamStateList[newState[0]]}");
        }

        [PunRPC]
        private void _RPC_SendHackerButtonClicked(int[] newState)
        {
            _playersTeamStateList[newState[0]] = (Team) newState[1];
            Debug.Log(
                $"_RPC_SendHackerButtonClicked:: {_playerList[newState[0]]}::{_playersTeamStateList[newState[0]]}");
        }

        #endregion

        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion
    }
}