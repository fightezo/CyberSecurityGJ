using GameModule.Class;
using ItemModule.Class;
using Photon.Pun;
using UnityEngine;

namespace PlayerModule.Class
{
    public class PlayerAnimatorManager : MonoBehaviourPun, IPunObservable
    {
        #region Private Fields

        private float _speedModifier = 0.1f;
        [SerializeField] private int _direction;

        #endregion

        #region MonoBehaviour Callbacks

        // Update is called once per frame
        private void Update()
        {
            // if (GameManager.Instance.GetGameState() != GameManager.GameState.Preparation &&
                // GameManager.Instance.GetGameState() != GameManager.GameState.Battle) return;
                if (GameManager.Instance.GetPlayerManager().GetIsFrozen()) return;
                if (photonView.IsMine && !ItemManager.Instance.MiniGameCanvas.activeSelf)
                {
                    var horizontalInputVal = Input.GetAxis("Horizontal") * _speedModifier;
                    if (horizontalInputVal > 0)
                    {
                        _direction = -1;
                    }
                    else if (horizontalInputVal < 0)
                    {
                        _direction = 1;
                    }

                    var verticalInputVal = Input.GetAxis("Vertical") * _speedModifier;
                    transform.localPosition += new Vector3(horizontalInputVal, 0f, verticalInputVal);
                }

                transform.localScale = new Vector3(_direction, 1f, 1f);
        }

        #endregion
        
                
        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext((int)_direction);
            }
            else
            {
                _direction = (int) stream.ReceiveNext();
            }
        }

        #endregion

    }
}