using UnityEngine;

namespace GameModule.Class
{
    public class CameraWork : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private Vector3 _centerOffset = Vector3.zero;
        private Transform _cameraTransform;
        private bool _isFollowing = false;
        private GameObject _target;
        #endregion


        #region MonoBehaviour Callbacks

        private void LateUpdate()
        {
            if (_isFollowing)
            {
                _Follow();
            }
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Raises the start following event.
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        public void OnStartFollowing(GameObject target)
        {
            _cameraTransform = Camera.main.transform;
            _isFollowing = true;
            _target = target;
            // we don't smooth anything, we go straight to the right camera shot
            _Cut();
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Follow the target smoothly
        /// </summary>
        private void _Follow()
        {
            _cameraTransform.position = _target.transform.position + _centerOffset;
        }


        private void _Cut()
        {
            _cameraTransform.position = _target.transform.position + _centerOffset;
        }

        #endregion
    }
}