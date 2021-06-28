using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private GameObject _startButton;

        [SerializeField] private Text _roomIdText;
        [SerializeField] private Text _countDownText;
        [SerializeField] private Slider _securityLevelSlider;

        [SerializeField] private GameObject _goodCitizenScenarioPanel;
        [SerializeField] private GameObject _hacerScenarioPanel;
        #endregion

        #region Private Fields

        private enum GameState
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
            {GameState.Planning, 60f},
            {GameState.Preparation, 60f},
            {GameState.Battle, 600f},
            {GameState.End, 120f},
        };

        private float _currentPlayTime = 0f;
        private GameState _currentState = GameState.WaitingForPlayers;
        private int _securityLevel = 10;
        private int _goodCitizenSecurityThreshold = 5;
        private int _hackerSecurityThreshold = -5;

        private PlayerManager _localPlayerManager;
        private float _timertoStartGame;

        private Player[] _playerList;
        private int _localPlayerIndex; 
        private bool _isGoodCitizenChosen;
        private string _goodCitizenUserId;
        
        private bool _isHackerChosen;
        private string _hackerUserId;
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
            if (_currentState != GameState.WaitingForPlayers || _currentState != GameState.End)
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
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName); // not seen if you're the player connecting
            _UpdatePlayerList();
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName); // seen when other disconnects

            _UpdatePlayerList();
            // TODO: You Win~
            LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        #region Public Methods
        public void CheckState(PlayerManager self, PlayerManager taggedPlayer)
        {
            Debug.Log("Check State");
            _localPlayerManager = self;
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

        public Team GetTeam(string name)
        {
            if (name == _goodCitizenUserId)
                return Team.GoodCitizen;
            if (name == _hackerUserId)
                return Team.Hacker;
            return Team.None;
        }
        
        public void OnStartGameButtonClicked()
        {
            if (!PhotonNetwork.IsMasterClient) return;
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

            _UpdateNextGameState();
        }

        public void OnGoodCitizenButtonClicked()
        {
            _isGoodCitizenChosen = !_isGoodCitizenChosen;
            photonView.RPC("_RPC_SendGoodCitizenButtonClicked", RpcTarget.AllBuffered, _isGoodCitizenChosen ? PhotonNetwork.LocalPlayer.NickName : string.Empty);
        }

        public void OnHackerButtonClicked()
        {
            _isHackerChosen = !_isHackerChosen;
            photonView.RPC("_RPC_SendHackerButtonClicked", RpcTarget.AllBuffered, _isHackerChosen ? PhotonNetwork.LocalPlayer.NickName : string.Empty);
        }
        #endregion

        #region Private Methods
        private void _FirstTimeSetup()
        {
            _playerList = PhotonNetwork.PlayerList;

            // _roomIdText.text = $"RoomID: {PhotonNetwork.CurrentRoom.Name}";
        }
        private void _UpdateViewPanel()
        {
            var currentIndex = (_currentState > GameState.Planning && _currentState < GameState.End ) ? 2 : (int) _currentState;
            Debug.Log($"panelList::{(GameState)currentIndex}");
            for (var index = 0; index < _phasePanelList.Count; index++)
            {
                _phasePanelList[index].SetActive( index == currentIndex);
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
        }

        private void _BeginPreparationPhase()
        {
            _currentState = GameState.Preparation;
            _currentPlayTime = TotalTimeList[_currentState];
        }

        private void _BeginBattlePhase()
        {
            _currentState = GameState.Battle;
            _currentPlayTime = TotalTimeList[_currentState];
            
            if (playerPrefab != null)
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    _localPlayerManager = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0).GetComponent<PlayerManager>();
                }
            }
        }

        private void _BeginEndPhase()
        {
            _currentState = GameState.End;
            _currentPlayTime = TotalTimeList[_currentState];
            //In the DarkWeb; capturing hacker
            if (_securityLevel >= _goodCitizenSecurityThreshold)
            {
                _DisplayGoodCitizenEnding();
            }
            //In Hacked Computer; steals identity
            if (_securityLevel <= _hackerSecurityThreshold)
            {
                _DisplayHackerEnding();
            }
        }

        private void _DisplayHackerEnding()
        {
            
        }

        private void _DisplayGoodCitizenEnding()
        {
            if (_localPlayerManager.GetState() == PlayerState.Invading)
            {
                
            }
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
                    _localPlayerIndex = index;
                _playerNameList[index].text = player.NickName;
            }

            _startButton.SetActive(_playerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers );
        }

        [PunRPC]
        private void _RPC_SendUpdatedGameState(GameState gameState)
        {
            _currentState = gameState;
            _UpdateViewPanel();
        }


        [PunRPC]
        private void _RPC_SendGoodCitizenButtonClicked(string name)
        {
            _goodCitizenUserId = name;
            Debug.Log($"_RPC_SendGoodCitizenButtonClicked:: {_goodCitizenUserId}");
        }

        [PunRPC]
        private void _RPC_SendHackerButtonClicked(string name)
        {
            _hackerUserId = name;
            Debug.Log($"_RPC_SendHackerButtonClicked:: {_hackerUserId}");

        }


        #endregion

        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion


    }
}