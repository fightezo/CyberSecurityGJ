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

        [SerializeField] private Text _roomIdText;
        [SerializeField] private Text _countDownText;
        [SerializeField] private Slider _securityLevelSlider;
        [SerializeField] private GameObject _goodCitizenScenarioPanel;
        [SerializeField] private GameObject _hacerScenarioPanel;
        #endregion

        #region Private Fields

        private enum GameState
        {
            Planning = 0,
            Preparation = 1,
            Battle = 2,
            End = 3,
        }

        private Dictionary<GameState, float> TotalTimeList = new Dictionary<GameState, float>()
        {
            {GameState.Planning, 60f},
            {GameState.Preparation, 60f},
            {GameState.Battle, 600f},
            {GameState.End, 120f},
        };

        private float _currentPlayTime = 0f;
        private GameState _currentState = GameState.Planning;
        private int _expectedPlayerNumber = 2;
        private int _securityLevel = 10;
        private int _goodCitizenSecurityThreshold = 5;
        private int _hackerSecurityThreshold = -5;

        private Player[] _playerList;
        
        private PlayerManager _localPlayerManager;
        private float _timertoStartGame;

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            Instance = this;
            _FirstTimeSetup();
            _ResetGameState();
        }

        private void Update()
        {
            if (_currentState != GameState.End)
            {
                _currentPlayTime -= Time.deltaTime;
                var timeLeft = TimeSpan.FromSeconds(_currentPlayTime);
                _countDownText.text = $"{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
            }

            if (_currentPlayTime <= 0f)
            {
                _UpdateNextGameState();
            }

            Debug.LogWarning(_playerList[0].GetPhotonTeam());
            Debug.LogWarning(_playerList[1].GetPhotonTeam());

        }

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
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

        #endregion

        #region Private Methods
        private void _FirstTimeSetup()
        {
            if (playerPrefab != null)
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    _localPlayerManager = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0).GetComponent<PlayerManager>();
                }
            }

            // _roomIdText.text = $"RoomID: {PhotonNetwork.CurrentRoom.Name}";
            _playerList = PhotonNetwork.PlayerList;
        }


        private void _UpdateNextGameState()
        {
            switch (_currentState)
            {
                // case GameState.WaitingForPlayers:
                //     _BeginPlanningPhase();
                //     break;
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
            _currentState = GameState.Planning;
            _currentPlayTime = TotalTimeList[_currentState];
        }


        #endregion

        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion


    }
}