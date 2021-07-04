using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemModule.Class.Component.UI
{
    public class MiniGamePanel : MonoBehaviour
    {
        private enum PasswordType
        {
            Good,
            Bad,
        }

        public GameObject MiniGameObject;
        public GameObject ResultGameObject;
        public Text PointEarnedText;
        public Text CountdownText;
        public Text PasswordDisplayText;
        public List<UIPlanningItem> UIPurchaseItemList;

        private List<string> _passwordList = new List<string>()
        {
            "aaa",
            "bbb",
        };

        private int _currentIndex;
        private float _currentPlayTime = 10f;
        private string _countDownText;

        public void Play()
        {
            MiniGameObject.SetActive(true);
            ResultGameObject.SetActive(false);
            _UpdatePasswordDisplay();
        }

        private void _UpdatePasswordDisplay()
        {
            var randomIndex = UnityEngine.Random.Range(0, _passwordList.Count);
            PasswordDisplayText.text = _passwordList[randomIndex];
        }

        public void Update()
        {
            _currentPlayTime -= Time.deltaTime;
            var timeLeft = TimeSpan.FromSeconds(_currentPlayTime);
            // CountdownText.text = $"{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
            CountdownText.text = $"{timeLeft.Seconds:00}";
            if (_currentPlayTime <= 0f)
            {
                _GoToResultPanel();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _CheckPassword(_currentIndex, PasswordType.Good);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _CheckPassword(_currentIndex, PasswordType.Bad);
            }
        }

        private void _GoToResultPanel()
        {
            MiniGameObject.SetActive(true);
            ResultGameObject.SetActive(false);
        }

        // TODO: Tiphanie, update PointEarnedText.text;
        private void _CheckPassword(int currentIndex, PasswordType passwordType)
        {
            throw new NotImplementedException();
        }

        public void OnConfirmButtonClicked()
        {
            // update ItemManager
            // will only choose the first x items;
        }
        
    }
}
