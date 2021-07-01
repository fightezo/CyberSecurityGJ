using System.Collections;
using System.Collections.Generic;
using GameModule.Class.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class.Component
{
    internal class PlanningPanel : MonoBehaviour, IUIPanel
    {
        public Text CountDownText;
        public List<Text> PlayerNameList;
        public GameObject Player0Group;
        public GameObject Player1Group;
        
        public GameObject GetSelf()
        {
            return gameObject;
        }
    }
}