// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraWork.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the Camera work to follow the player
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;

namespace GameModule.Class
{
    /// <summary>
    /// Camera work. Follow a target
    /// </summary>
    public class CameraWork : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("The distance in the local x-z plane to the target")] [SerializeField]
        private float _distance = 7.0f;

        [Tooltip("The height we want the camera to be above the target")] [SerializeField]
        private float _height = 3.0f;

        [Tooltip(
            "Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
        [SerializeField]
        private Vector3 _centerOffset = Vector3.zero;


        [Tooltip(
            "Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
        [SerializeField]
        private bool _followOnStart = false;


        [Tooltip("The Smoothing for the camera to follow the target")] [SerializeField]
        private float _smoothSpeed = 0.125f;


        // cached transform of the target
        private Transform _cameraTransform;


        // maintain a flag internally to reconnect if target is lost or camera is switched
        private bool _isFollowing;


        // Cache for camera offset
        private Vector3 _cameraOffset = Vector3.zero;

        #endregion


        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (_followOnStart)
            {
                OnStartFollowing();
            }
        }

        private void LateUpdate()
        {
            // The transform target may not destroy on level load,
            // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
            if (_cameraTransform == null && _isFollowing)
            {
                OnStartFollowing();
            }


            // only follow is explicitly declared
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
        public void OnStartFollowing()
        {
            _cameraTransform = Camera.main.transform;
            _isFollowing = true;
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
            _cameraOffset.z = -_distance;
            _cameraOffset.y = _height;


            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                this.transform.position + this.transform.TransformVector(_cameraOffset), _smoothSpeed * Time.deltaTime);


            _cameraTransform.LookAt(this.transform.position + _centerOffset);
        }


        private void _Cut()
        {
            _cameraOffset.z = -_distance;
            _cameraOffset.y = _height;


            _cameraTransform.position = this.transform.position + this.transform.TransformVector(_cameraOffset);


            _cameraTransform.LookAt(this.transform.position + _centerOffset);
        }

        #endregion
    }
}