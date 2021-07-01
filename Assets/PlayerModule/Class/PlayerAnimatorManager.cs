using Photon.Pun;
using UnityEngine;

namespace PlayerModule.Class
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields

        private float _speedModifier = 0.1f;

        #endregion

        #region MonoBehaviour Callbacks

        // Use this for initialization
        private void Start()
        {
        }


        // Update is called once per frame
        private void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
                return;

            var horizontalInputVal = Input.GetAxis("Horizontal") * _speedModifier;
            var verticalInputVal = Input.GetAxis("Vertical") * _speedModifier;

            transform.localPosition += new Vector3(horizontalInputVal, 0f, verticalInputVal);
        }

        #endregion
    }
}