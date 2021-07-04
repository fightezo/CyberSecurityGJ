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
            "good|9Lxj7P6U-6^kyphF",
            "good|N_N5n!G&6F`.mq9r",
            "good|[H{=jW/`6A:x=U?h",
            "good|Tk6Cd^,&t:jg-f>$",
            "good|m@'An^da^r9y]]zb",
            "good|5dwtRe`5Ja+a),]s",
            "good|W8M`dMqBjdB^]cq%",
            "good|anEG~zx%HWb']3@S",
            "good|/LB+-dZdWMX=wY7z",
            "good|f`8uNBvAPVb&^<R",
            "good|dgJL_w6))+%myYK-",
            "good|k]f5Sx):AsXV;52{",
            "good|;h^YkukWk@4pVdqJ",
            "good|ftzE8<7T]GP*ZUh",
            "good|42&8{kL^8Qw(Qv<g",
            "good|J?uE5<FvpZ#yFNc*",
            "good|)_@zsegFT)WEXX2#",
            "good|`_q64x`nDe6:^@BB",
            "good|LHYCP5sDU93[g3^C",
            "good|EC/g9&GvpYT>L/M",
            "bad|123456",
            "bad|123456789",
            "bad|picture1",
            "bad|password",
            "bad|12345678",
            "bad|111111",
            "bad|123123",
            "bad|12345",
            "bad|1234567890",
            "bad|senha",
            "bad|1234567",
            "bad|qwerty",
            "bad|abc123",
            "bad|Million2",
            "bad|000000",
            "bad|1234",
            "bad|iloveyou",
            "bad|aaron431",
            "bad|password1",
            "bad|qqww1122",
        };

        private int _currentIndex;
        private int _currentPoints;
        private float _currentPlayTime = 10f;
        private string _countDownText;
        private bool _startGame;

        public void Play()
        {
            _startGame = true;
            MiniGameObject.SetActive(true);
            ResultGameObject.SetActive(false);
            _currentPoints = 0;
            _currentPlayTime = 10f;
            _UpdatePasswordDisplay();
            
        }

        public void Start()
        {
            
        }

        private void _UpdatePasswordDisplay()
        {
            var randomIndex = UnityEngine.Random.Range(0, _passwordList.Count);
            _currentIndex = randomIndex;
            var splitData = _passwordList[_currentIndex].Split('|');

            PasswordDisplayText.text = splitData[1];
        }

        public void Update()
        {
            if (!_startGame) return;
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
                _UpdatePasswordDisplay();
                PointEarnedText.text = _currentPoints.ToString();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _CheckPassword(_currentIndex, PasswordType.Bad);
                _UpdatePasswordDisplay();
                PointEarnedText.text = _currentPoints.ToString();
            }

        }

        private void _GoToResultPanel()
        {
            MiniGameObject.SetActive(false);
            ResultGameObject.SetActive(true);
            _startGame = false;
            
        }

        // TODO: Tiphanie, update PointEarnedText.text;
        private void _CheckPassword(int currentIndex, PasswordType passwordType)
        {
            var splitData = _passwordList[currentIndex].Split('|');
            if (passwordType == PasswordType.Bad && splitData[0] == "bad")
            {
                _currentPoints++;
                return;
            }

            if (passwordType == PasswordType.Good && splitData[0] == "good")
            {
                _currentPoints++;
                return;
            }

            _currentPoints--;
        }

        public void OnConfirmButtonClicked()
        {
            // update ItemManager
            // will only choose the first x items;
        }
        
    }
}
