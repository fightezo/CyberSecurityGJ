using System.Collections;
using System.Collections.Generic;
using GameModule.Class.Component.UI;
using GameModule.Class.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class.Component
{
    internal class GameplayPanel : MonoBehaviour, IUIPanel
    {
        public Text CountDownText;
        public Slider SecurityLevelSlider;
        public GameManager.GameState _currentGameState;

        public List<UIGameplayChest> UIGameplayChestList;
        public List<UIGameplayTool> UIGameplayToolList;
        public GameObject GetSelf()
        {
            return gameObject;
        }

        public void UpdateView(GameManager.GameState gameState, int defenderSecurityMax, int hackerSecurityMax, int securityLevel)
        {
            _currentGameState = gameState;
            SecurityLevelSlider.minValue = hackerSecurityMax;
            SecurityLevelSlider.maxValue = defenderSecurityMax;
            SecurityLevelSlider.value = securityLevel;
            // _securityLevelSlider.onValueChanged.AddListener(UpdateSecurityLevel);
        }
        
        public void EndPhaseUpdate()
        {
            if (_currentGameState == GameManager.GameState.Preparation)
            {
                
            }

            if (_currentGameState == GameManager.GameState.Battle)
            {
                
            }
        }

    }
}