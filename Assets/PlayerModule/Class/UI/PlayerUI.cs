using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerModule.Class.UI
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("UI Text to display Player's Name")] [SerializeField]
        private Text playerNameText;


        [Tooltip("UI Slider to display Player's Health")] [SerializeField]
        private Slider playerHealthSlider;

        [Tooltip("Pixel offset from the player target")] [SerializeField]
        private Vector3 _screenOffset = new Vector3(0f, 30f, 0f);

        private PlayerManager _target;
        private float _characterControllerHeight = 0f;
        private Transform _targetTransform;
        private Renderer _targetRenderer;
        private CanvasGroup _canvasGroup;
        private Vector3 _targetPosition;
        private Camera _camera;

        #endregion


        #region MonoBehaviour Callbacks

        private void Start()
        {
            _camera = Camera.main;
        }

        public void Awake()
        {
            transform.SetParent(GameObject.FindWithTag("MainCanvas").GetComponent<Transform>(), false);
            _canvasGroup = GetComponent<CanvasGroup>();
            
        }

        public void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = _target.Health;
            }
        }

        public void LateUpdate()
        {
            if (_targetRenderer != null)
            {
                _canvasGroup.alpha = _targetRenderer.isVisible ? 1f : 0f;
            }

            if (_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;
                _targetPosition.y += _characterControllerHeight;
                transform.position = _camera.WorldToScreenPoint(_targetPosition) + _screenOffset;
            }
        }

        #endregion


        #region Public Methods

        public void SetTarget(PlayerManager target)
        {
            if (target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.",
                    this);
                return;
            }

            _target = target;
            _targetTransform = _target.transform;
            _targetRenderer = _target.GetComponent<Renderer>();
            var characterController = _target.GetComponent<CharacterController>();
            if (characterController != null)
            {
                _characterControllerHeight = characterController.height;
            }
            
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
        }

        #endregion
    }
}