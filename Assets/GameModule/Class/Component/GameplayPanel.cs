using System.Collections;
using System.Collections.Generic;
using GameModule.Class.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class.Component
{
    internal class GameplayPanel : MonoBehaviour, IUIPanel
    {
        public Text CountDownText;
        public Slider SecurityLevelSlider;
    
        public GameObject GetSelf()
        {
            return gameObject;
        }
    }
}