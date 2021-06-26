using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using PlayerModule.Class;
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
            {GameState.WaitingForPlayers, 0f},
            {GameState.Planning, 60f},
            {GameState.Preparation, 60f},
            {GameState.Battle, 600f},
            {GameState.End, 60f},
        };

        private float _currentPlayTime = 0f;
        private GameState _currentState = GameState.WaitingForPlayers;
        private int _expectedPlayerNumber = 2;
        private int _securityLevel = 10;
        private int _goodCitizenSecurityThreshold = 5;
        private int _hackerSecurityThreshold = -5;
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

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}",
                    PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                // _LoadArena();
                if (PhotonNetwork.CurrentRoom.PlayerCount == _expectedPlayerNumber)
                {
                    _currentPlayTime = 0f;
                }
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}",
                    PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                // _LoadArena();
            }
        }

        #endregion

        #region Public Methods
        public void CheckState(PlayerManager self, PlayerManager taggedPlayer)
        {
            Debug.Log("Check State");
            if (_currentState == GameState.Battle)
            {
                //In the DarkWeb; capturing hacker
                if (_securityLevel >= _goodCitizenSecurityThreshold)
                {
                    _UpdateNextGameState();
                }
                //In Hacked Computer; steals identity
                if (_securityLevel <= _hackerSecurityThreshold)
                {
                    _UpdateNextGameState();
                }
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
                    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
            }

            _roomIdText.text = $"RoomID: {PhotonNetwork.CurrentRoom.Name}";
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
        }

        private void _BeginAfterEnd()
        {
        }

        private void _ResetGameState()
        {
            _currentState = GameState.WaitingForPlayers;
            _currentPlayTime = TotalTimeList[_currentState];
        }

        private void _LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            // PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
            // PhotonNetwork.LoadLevel("MazeRoom");
        }

        #endregion

        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion


    }
}