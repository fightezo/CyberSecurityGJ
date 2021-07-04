using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameModule.Class;
using ItemModule;
using ItemModule.Class;
using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using MapModule.Class;
using Photon.Realtime;
using PlayerModule.Class.Data;
using PlayerModule.Class.UI;
using Debug = UnityEngine.Debug;

namespace PlayerModule.Class
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        [SerializeField] public GameObject PlayerUiPrefab;

        public Renderer Renderer;
        public GameObject RobotGameObject;
        public GameObject CitizenGameObject;
        public GameObject HackerGameObject;
        public GameObject PressEButtonGameObject;
        #endregion

        #region Private Fields

        private IItem _itemInRange;
        private Collider _computerInRange;
        private Collider _slotPointInRange;
        private bool _isPlayerInRange;

        private List<int> _itemList = new List<int>();
        private PlayerManager _taggedPlayer;
        private Team _team;
        private PlayerState _currentState;
        private bool isLocalTesting;
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (photonView.IsMine )
            {
                LocalPlayerInstance = gameObject;
            }

            _SetIsLocalTesting();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += _OnSceneLoaded;
        }

        public void UpdatePlayerData(Vector3 spawnPosition, Team team)
        {
            var cameraWork = gameObject.GetComponent<CameraWork>();
            if (cameraWork != null)
            {
                if (photonView.IsMine && !isLocalTesting)
                {
                    cameraWork.OnStartFollowing(gameObject);
                }
            }

            if (photonView.IsMine && !isLocalTesting)
            {
                _team = team;
                photonView.RPC("RPC_SetTeam", RpcTarget.AllBuffered, (int)_team);
                gameObject.transform.position = spawnPosition;
            }

            if (PlayerUiPrefab != null)
            {
                _CreatePlayerUi();
            }
        }

        private void _UpdateTeamView()
        {
            CitizenGameObject.SetActive(_team == Team.Defender);
            HackerGameObject.SetActive(_team == Team.Hacker);
        }


        private void Update()
        {
            if (photonView.IsMine)
            {
                _ProcessInputs();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (_currentState != PlayerState.Invading)
            {
                // interact with items
                if (other.CompareTag("Item"))
                {
                    PressEButtonGameObject.SetActive(true);
                    _itemInRange = other.gameObject.GetComponent<IItem>();
                }

                // play miniGame
                if (other.CompareTag("ComputerPoint"))
                {
                    _computerInRange = other;
                }

                //
                if (other.CompareTag("SlotPoint"))
                {
                    _slotPointInRange = other;
                }
            }
            else
            {
                if (other.CompareTag("Player"))
                {
                    _isPlayerInRange = true;
                }
                 
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            

            if (_currentState != PlayerState.Invading)
            {
                // interact with items
                if (other.CompareTag("Item"))
                {
                    PressEButtonGameObject.SetActive(false);
                    _itemInRange = null;

                } 
                // play miniGame
                if (other.CompareTag("GetItemPoint"))
                {
                    _computerInRange = null;
                }

                //
                if (other.CompareTag("SlotPoint"))
                {
                    _slotPointInRange = null;
                }
            }
            else
            {
                if (other.CompareTag("Player"))
                {
                    _isPlayerInRange = false;
                }
                 
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            // if (other.gameObject.CompareTag("Player"))
            // {
            //     GameManager.Instance.CheckState();
            // }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneLoaded -= _OnSceneLoaded;
        }
        #endregion
        
        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // stream.SendNext(IsFiring);
                // stream.SendNext(itemList.ToArray());
            }
            else
            {
                // IsFiring = (bool) stream.ReceiveNext();
                // itemList = ((int[]) stream.ReceiveNext()).ToList();
            }
        }

        #endregion

        #region Public Methods

        public PlayerState GetState()
        {
            return _currentState;
        }

        public Team GetTeam()
        {
            return _team;
        }
        #endregion
        #region Private Methods
        private void _CreatePlayerUi()
        {
            var uiGameObject = Instantiate(PlayerUiPrefab).GetComponent<PlayerUI>();
            uiGameObject.SetTarget(this);
        }

        private void _ProcessInputs()
        {
            if (Input.GetKeyDown(KeyCode.T) && GameManager.Instance.IsReadyToTeleport(_team))
            {
                Teleport();
            }
            // Interact with Item
            if (_itemInRange != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ItemManager.Instance.PopupItemUI();
                }
            }
            // Interact with Computer
            if (_computerInRange != null)
            {
                if (Input.GetKeyDown(KeyCode.E) && !ItemManager.Instance.MiniGameCanvas.activeSelf)
                {
                    ItemManager.Instance.PopupMiniGameUI();
                } 
                if (Input.GetKeyDown(KeyCode.Escape) && ItemManager.Instance.MiniGameCanvas.activeSelf)
                {
                    ItemManager.Instance.DismissMiniGameUI();
                }
            }
            // Destroy Enemy
            if (Input.GetKeyDown(KeyCode.Space) && _isPlayerInRange)
            {
                GameManager.Instance.CheckState();
            }

            // Interact with Slot
            if (_slotPointInRange != null)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    ItemManager.Instance.PlaceItemOnSlotPoint(0, _slotPointInRange);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    ItemManager.Instance.PlaceItemOnSlotPoint(1, _slotPointInRange);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    ItemManager.Instance.PlaceItemOnSlotPoint(2, _slotPointInRange);
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    ItemManager.Instance.PlaceItemOnSlotPoint(3, _slotPointInRange);
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    ItemManager.Instance.PlaceItemOnSlotPoint(4, _slotPointInRange);
                }
            }

            // Use Item any time
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ItemManager.Instance.UseTool(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ItemManager.Instance.UseTool(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ItemManager.Instance.UseTool(2);
            }
        }

        public void Teleport(bool isForceReturn = false)
        {
            if (photonView.IsMine)
            {
                if (_currentState == PlayerState.Invading || isForceReturn)
                {
                    _currentState = PlayerState.Normal;
                    if (_team == Team.Defender)
                    {
                        transform.localPosition -= MapManager.Instance.GetTranslationToHackerMap();
                    }
                    if (_team == Team.Hacker)
                    {
                        transform.localPosition -= MapManager.Instance.GetTranslateToDefenderMap();
                    }  
                }
                else
                {
                    _currentState = PlayerState.Invading;
                    if (_team == Team.Defender)
                    {
                        transform.localPosition += MapManager.Instance.GetTranslationToHackerMap();
                    }
                    if (_team == Team.Hacker)
                    {
                        transform.localPosition += MapManager.Instance.GetTranslateToDefenderMap();
                    } 
                }
            }
        }

        private void _OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _CalledOnLevelWasLoaded(scene.buildIndex);
        }

        private void _CalledOnLevelWasLoaded(int level)
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 10f))
            {
                transform.position = new Vector3(0f, 10f, 0f);
            }

            _CreatePlayerUi();
        }
        
        private void _GetItem()
        {
            photonView.RPC("RPC_SendItemList", RpcTarget.Others, _itemList);
        }

        [PunRPC]
        private void RPC_SetTeam(int team)
        {
            _team = (Team)team;
            _UpdateTeamView();
        }
        [PunRPC]
        private void RPC_SendItemList(List<int> itemList)
        {
            _itemList = itemList;
        }

        [Conditional("UNITY_EDITOR")]
        private void _SetIsLocalTesting()
        {
            isLocalTesting = GameManager.Instance == null;
        }
        #endregion


    }
}