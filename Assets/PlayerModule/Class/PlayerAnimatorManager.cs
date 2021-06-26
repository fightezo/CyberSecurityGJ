using Photon.Pun;
using UnityEngine;

namespace PlayerModule.Class
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields

        [SerializeField] private float directionDampTime = 0.25f;

        #endregion

        private Animator _animator;

        #region MonoBehaviour Callbacks

        // Use this for initialization
        private void Start()
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }


        // Update is called once per frame
        private void Update()
        {
            if(!photonView.IsMine && PhotonNetwork.IsConnected)
                return;
            
            if (_animator == null)
            {
                return;
            }

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    _animator.SetTrigger("Jump");
                }
            }
            var horizontalInputVal = Input.GetAxis("Horizontal");
            var verticalInputVal = Input.GetAxis("Vertical");
            if (verticalInputVal < 0)
            {
                // verticalInputVal = 0;
            }

            _animator.SetFloat("Speed", horizontalInputVal * horizontalInputVal + verticalInputVal * verticalInputVal);
            _animator.SetFloat("Direction", horizontalInputVal, directionDampTime, Time.deltaTime);
        }

        #endregion
    }
}