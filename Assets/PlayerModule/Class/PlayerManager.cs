using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameModule.Class;
using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using Photon.Realtime;
using PlayerModule.Class.Data;
using PlayerModule.Class.UI;

namespace PlayerModule.Class
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        
        [Tooltip("The Player's UI GameObject Prefab")][SerializeField]
        public GameObject PlayerUiPrefab;
        
        #endregion

        #region Private Fields

        private bool _withinItemRange;
        private List<int> _itemList = new List<int>();
        private PlayerManager _taggedPlayer;
        private PlayerModule.Class.Data.Team _team;
        private PlayerModule.Class.Data.PlayerState _currentState;
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += _OnSceneLoaded;
            // var cameraWork = gameObject.GetComponent<CameraWork>();
            // if (cameraWork != null)
            // {
                // if (photonView.IsMine)
                // {
                    // cameraWork.OnStartFollowing();
                // }
            // }

            if (PlayerUiPrefab != null)
            {
                _CreatePlayerUi();
            }
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

            if (other.CompareTag("Item"))
            {
                var item = other.gameObject.GetComponent<IItem>();
                if (item.GetItemState() == ItemState.World)
                {
                    _GetItem();
                }
                _withinItemRange = true;
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
            
            if (other.CompareTag("Item"))
            {
                _withinItemRange = false;
            } 

        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (hit.collider.CompareTag("Player"))
            {
                GameManager.Instance.CheckState(this, _taggedPlayer);
            }
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
        public void SetupPlayerManager(Data.Team team)
        {
            _team = team;
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
            if (Input.GetKeyDown(KeyCode.E) && _withinItemRange)
            {
                
            }
        }

        private void _OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _CalledOnLevelWasLoaded(scene.buildIndex);
        }

        private void _CalledOnLevelWasLoaded(int level)
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            _CreatePlayerUi();
        }
        
        private void _GetItem()
        {
            photonView.RPC("_RPC_SendItemList", RpcTarget.Others, _itemList);
        }
        
        [PunRPC]
        private void _RPC_SendItemList(List<int> itemList)
        {
            _itemList = itemList;
        }

        #endregion


    }
}