using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using GameModule.Class.Component;
using GameModule.Class.Interface;
using ItemModule.Class.Data;
using MapModule.Class;
using Photon.Pun;
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

        [SerializeField] private WaitingPanel WaitingPanel;
        [SerializeField] private PlanningPanel PlanningPanel;
        [SerializeField] private GameplayPanel GameplayPanel;
        [SerializeField] private EndPanel EndPanel;
        // private IUIPanel _currentPanel;

        [SerializeField] private List<GameObject> _phasePanelList;
        [SerializeField] private Text _roomIdText;

        // planning phase

        // preparation & gameplay phase
        [SerializeField] private Text _countDownText;
        [SerializeField] private Slider _securityLevelSlider;
        [SerializeField] private int _securityLevel = 0;

        // end phase
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
            {GameState.Planning, 20f},
            {GameState.Preparation, 30f},
            {GameState.Battle, 600f},
            {GameState.End, 120f},
        };

        private Dictionary<GameState, IUIPanel> PanelList = new Dictionary<GameState, IUIPanel>();
        
        private float _currentPlayTime = 0f;
        private GameState _currentState = GameState.WaitingForPlayers;

        private int _defenderSecurityThreshold = 5;
        private int _defenderSecurityMax = 10;
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

            PanelList = new Dictionary<GameState, IUIPanel>()
            {
                {GameState.WaitingForPlayers, WaitingPanel},
                {GameState.Planning, PlanningPanel},
                {GameState.Preparation, GameplayPanel},
                {GameState.Battle, GameplayPanel},
                {GameState.End, EndPanel},
            };
        }

        private void Update()
        {
            if (_currentState != GameState.WaitingForPlayers && _currentState != GameState.End)
            {
                _currentPlayTime -= Time.deltaTime;
                var timeLeft = TimeSpan.FromSeconds(_currentPlayTime);
                _countDownText.text = $"{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
                PlanningPanel.CountDownText.text = _countDownText.text;
                GameplayPanel.CountDownText.text = _countDownText.text;
            }

            if (_currentPlayTime <= 0f)
            {
                GetNextGameState();
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

        public IUIPanel GetPanel()
        {
            return PanelList[_currentState];
        }
        public void CheckState()
        {
            Debug.Log("Check State");
            if (_currentState == GameState.Battle)
            {
                GetNextGameState();
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

        public void UpdateGame(float newVal)
        {
            if(!IsReadyToTeleport(_localPlayerManager.GetTeam()))
            {
                _localPlayerManager.Teleport(true);
            }
        }

        public bool IsReadyToTeleport(Team team)
        {
            if (team == Team.Defender && _securityLevel >= _defenderSecurityThreshold)
            {
                return true;
            }

            if (team == Team.Hacker && _securityLevel <= _hackerSecurityThreshold)
            {
                return true;
            }

            return false;
        }

        public void UpdateGameplayUIView()
        {
            GameplayPanel.UpdateUIView();
        }
        #endregion

        #region Public Button Methods

        public void OnItemClicked()
        {
        }

        #endregion

        #region Private Methods

        private void _FirstTimeSetup()
        {
            _roomIdText.text = $"{PhotonNetwork.CurrentRoom.Name}";

        }

        private void _UpdateViewPanel()
        {
            // var currentIndex = (_currentState > GameState.Planning && _currentState < GameState.End ) ? 2 : (int) _currentState;
            var currentIndex = (int) _currentState;
            Debug.Log($"panelList::{(GameState) currentIndex}");
            var currentPhase = 0;
            switch (_currentState)
            {
                case GameState.WaitingForPlayers:
                    currentPhase = 0;
                    break;
                case GameState.Planning:
                    currentPhase = 1;
                    break;
                case GameState.Preparation:
                case GameState.Battle:
                    currentPhase = 2;
                    break;
                case GameState.End:
                    currentPhase = 3;

                    break;
            }
            
            for (var index = 0; index < _phasePanelList.Count; index++)
            {
                _phasePanelList[index].SetActive(index == currentPhase);
            }
        }

        protected internal void GetNextGameState()
        {
            PanelList[_currentState].EndPhaseUpdate();
            var _nextState = GameState.WaitingForPlayers;
            switch (_currentState)
            {
                case GameState.WaitingForPlayers:
                    _nextState = GameState.Planning;
                    break;
                case GameState.Planning:
                    _nextState = GameState.Preparation;
                    break;
                case GameState.Preparation:
                    _nextState = GameState.Battle;
                    break;
                case GameState.Battle:
                    _nextState = GameState.End;
                    break;
            }

            photonView.RPC("RPC_SendUpdatedGameState", RpcTarget.AllBuffered, _nextState);
        }

        protected internal void UpdateNewGameState()
        {
            switch (_currentState)
            {
                case GameState.Planning:
                    _BeginPlanningPhase();
                    break;
                case GameState.Preparation:
                    _BeginPreparationPhase();
                    break;
                case GameState.Battle:
                    _BeginBattlePhase();
                    break;
                case GameState.End:
                    _BeginEndPhase();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            // switch (_currentState)
            // {
            //     // case GameState.WaitingForPlayers:
            //          _BeginPlanningPhase();
            //         break;
            //     case GameState.Planning:
            //         _BeginPreparationPhase();
            //         break;
            //     case GameState.Preparation:
            //         _BeginBattlePhase();
            //         break;
            //     case GameState.Battle:
            //         _BeginEndPhase();
            //         break;
            //     case GameState.End:
            //         // _BeginAfterEnd();
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }


        private void _BeginPlanningPhase()
        {
            PlanningPanel.UpdateView(_playersTeamStateList[_localPlayerIndex]);
        }

        private void _BeginPreparationPhase()
        {
            GameplayPanel.UpdateView(GameState.Preparation, _defenderSecurityMax, _hackerSecurityMax, _securityLevel);
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
            GameplayPanel.UpdateView(GameState.Battle, _defenderSecurityMax, _hackerSecurityMax, _securityLevel);
        }

        private void _BeginEndPhase()
        {
            EndPanel.UpdateView(_securityLevel, _defenderSecurityThreshold, _hackerSecurityThreshold, _localPlayerManager);
        }


        private void _BeginAfterEnd()
        {
        }

        private void _ResetGameState()
        {
            _currentState = GameState.WaitingForPlayers;
            _currentPlayTime = TotalTimeList[_currentState];
            
            WaitingPanel.ResetData();
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
                    // WaitingPanel.Player0ButtonGroup.SetActive(index == 0);
                    // WaitingPanel.Player1ButtonGroup.SetActive(index == 1);
                }

                WaitingPanel.PlayerNameList[index].text = player.NickName;
                WaitingPanel.UpdateData(_localPlayerIndex);
            }

            _CreateLocalPlayerInstance();
            WaitingPanel.StartButton.SetActive(_playerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers);
        }


        #endregion

        #region PUNRPC Methods

        [PunRPC]
        private void RPC_UpdatePlayerData()
        {
            Debug.Log($"{name}:: RPC_UpdatePlayerData called;");
            var position = Vector3.zero;

            if (_playersTeamStateList[_localPlayerIndex] == Team.Defender)
            {
                position = MapManager.Instance.GetDefenderSpawnPoint();
            }
            else if (_playersTeamStateList[_localPlayerIndex] == Team.Hacker)
            {
                position = MapManager.Instance.GetHackerSpawnPoint();
            }

            _localPlayerManager.UpdatePlayerData(position, _playersTeamStateList[_localPlayerIndex]);

        }

        [PunRPC]
        private void RPC_SendUpdatedGameState(GameState gameState)
        {
            _currentState = gameState;
            _currentPlayTime = TotalTimeList[_currentState];
            UpdateNewGameState();
            _UpdateViewPanel();
        }

        [PunRPC]
        private void RPC_SendDefenderButtonClicked(int[] newState)
        {
            _playersTeamStateList[newState[0]] = (Team) newState[1];
            WaitingPanel.UpdatePlayerTeam(newState);

            Debug.Log(
                $"RPC_SendDefenderButtonClicked:: {_playerList[newState[0]]}::{_playersTeamStateList[newState[0]]}");
        }

        [PunRPC]
        private void RPC_SendHackerButtonClicked(int[] newState)
        {
            _playersTeamStateList[newState[0]] = (Team) newState[1];
            WaitingPanel.UpdatePlayerTeam(newState);
            Debug.Log(
                $"RPC_SendHackerButtonClicked:: {_playerList[newState[0]]}::{_playersTeamStateList[newState[0]]}");
        }

        [PunRPC]
        private void RPC_UpdateSecurityLevel(int newChangedValue)
        {
            _securityLevel += newChangedValue;
            GameplayPanel.SecurityLevelSlider.value = _securityLevel;
        }
        #endregion

        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion

        internal List<Team> GetPlayersTeamStateList()
        {
            return _playersTeamStateList;
        }

        internal Player[] GetPlayersList()
        {
            return _playerList;
        }

        internal int GetLocalPlayerIndex()
        {
            return _localPlayerIndex;
        }
        internal PlayerManager GetPlayerManager()
        {
            return _localPlayerManager;
        }

        //Tool Methods
        public void SendAttacker()
        {
            throw new NotImplementedException();
        }
    }
}