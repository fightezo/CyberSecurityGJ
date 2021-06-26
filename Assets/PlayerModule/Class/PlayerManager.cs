using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameModule.Class;
using ItemModule.Class.Interface;
using Photon.Realtime;
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
        private List<int> itemList = new List<int>();
        private PlayerManager _taggedPlayer;

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
                if (_taggedPlayer != null)
                {
                    GameManager.Instance.CheckState(this, _taggedPlayer);
                }

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
                _withinItemRange = true;
            }

            if (other.CompareTag("Player"))
            {
                _taggedPlayer = other.gameObject.GetComponent<PlayerManager>();
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
            if (other.CompareTag("Player"))
            {
                _taggedPlayer = null;
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
                stream.SendNext(itemList);
            }
            else
            {
                // IsFiring = (bool) stream.ReceiveNext();
                itemList = (List<int>) stream.ReceiveNext();
            }
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

        #endregion


    }
}