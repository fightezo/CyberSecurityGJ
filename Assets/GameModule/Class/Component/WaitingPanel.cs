using System;
using System.Collections;
using System.Collections.Generic;
using GameModule.Class.Interface;
using Photon.Pun;
using PlayerModule.Class.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class.Component
{
    internal class WaitingPanel : MonoBehaviour, IUIPanel
    {

        // waiting phase
        public List<Text> PlayerNameList;
        public GameObject Player0ButtonGroup;
        public GameObject Player1ButtonGroup;
        public GameObject StartButton;
        public Toggle Player0DefenderButton;
        public Toggle Player0HackerButton;
        public Toggle Player1DefenderButton;
        public Toggle Player1HackerButton;

        private int _localPlayerIndex;

        public void ResetData()
        {
            StartButton.SetActive(PhotonNetwork.IsMasterClient);
        }
        
        public void UpdateData(int localPlayerIndex)
        {
            _localPlayerIndex = localPlayerIndex;
            if (_localPlayerIndex == 0)
            {
                Player0DefenderButton.interactable = true;
                Player0HackerButton.interactable = true;
                
                Player1DefenderButton.interactable = false;
                Player1HackerButton.interactable = false;
            }

            if (_localPlayerIndex == 1)
            {
                Player0DefenderButton.interactable = false;
                Player0HackerButton.interactable = false;
                
                Player1DefenderButton.interactable = true;
                Player1HackerButton.interactable = true;
            }
        }

        public void OnStartGameButtonClicked()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (GameManager.Instance.GetPlayersTeamStateList().Contains(Team.None)) return;
            GameManager.Instance.GetNextGameState();
        }

        public void OnDefenderButtonClicked(bool isOn)
        {
            if (GameManager.Instance.GetPlayersTeamStateList().Contains(Team.Defender) &&
                GameManager.Instance.GetPlayersTeamStateList().FindIndex(x => x == Team.Defender) != _localPlayerIndex)
            {
                if (_localPlayerIndex == 0)
                {
                    Player0DefenderButton.SetIsOnWithoutNotify(false);
                } 
                if (_localPlayerIndex == 1)
                {
                    Player1DefenderButton.SetIsOnWithoutNotify(false);
                }


                return;
            }
            if (isOn)
            {
                if (_localPlayerIndex == 0)
                {
                    Player0HackerButton.SetIsOnWithoutNotify(false);
                } 
                if (_localPlayerIndex == 1)
                {
                    Player1HackerButton.SetIsOnWithoutNotify(false);
                }

     
            }
 
            var newState = GameManager.Instance.GetPlayersTeamStateList()[_localPlayerIndex];
            switch (newState)
            {
                case Team.None:
                case Team.Hacker:
                    newState = Team.Defender;
                    break;
                case Team.Defender:
                    newState = Team.None;
                    break;
            }

            var itemList = new[] {_localPlayerIndex, (int) newState};
            GameManager.Instance.photonView.RPC("_RPC_SendDefenderButtonClicked", RpcTarget.AllBuffered, itemList);
        }

        public void OnHackerButtonClicked(bool isOn)
        {
            if (GameManager.Instance.GetPlayersTeamStateList().Contains(Team.Hacker) &&
                GameManager.Instance.GetPlayersTeamStateList().FindIndex(x => x == Team.Hacker) != _localPlayerIndex)
            {
              
                if (_localPlayerIndex == 0)
                {
                    Player0HackerButton.SetIsOnWithoutNotify(false);
                } 
                if (_localPlayerIndex == 1)
                {
                    Player1HackerButton.SetIsOnWithoutNotify(false);
                }

                return;
            }
            if (isOn)
            {
                if (_localPlayerIndex == 0)
                {
                    Player0DefenderButton.SetIsOnWithoutNotify(false);
                }
                if (_localPlayerIndex == 1)
                {
                    Player1DefenderButton.SetIsOnWithoutNotify(false);
                }
            }

            var newState = GameManager.Instance.GetPlayersTeamStateList()[_localPlayerIndex];
            switch (newState)
            {
                case Team.None:
                case Team.Defender:
                    newState = Team.Hacker;
                    break;
                case Team.Hacker:
                    newState = Team.None;
                    break;
            }
            var itemList = new[] {_localPlayerIndex, (int) newState};
            GameManager.Instance.photonView.RPC("_RPC_SendHackerButtonClicked", RpcTarget.AllBuffered, itemList);
        }

        public GameObject GetSelf()
        {
            return gameObject;
        }

        public void EndPhaseUpdate()
        {
        }


        public void UpdatePlayerTeam(int[] playerData)
        {
            var newState = (Team) playerData[1];
            if (playerData[0] == 0)
            {
                switch (newState)
                {
                    case Team.Defender:
                        Player0DefenderButton.SetIsOnWithoutNotify(true);
                        Player0HackerButton.SetIsOnWithoutNotify(false);
                        break;
                    case Team.Hacker:
                        Player0DefenderButton.SetIsOnWithoutNotify(false);
                        Player0HackerButton.SetIsOnWithoutNotify(true);
                        break;
                    case Team.None:
                        Player0DefenderButton.SetIsOnWithoutNotify(false);
                        Player0HackerButton.SetIsOnWithoutNotify(false);
                        break;
                } 
            }

            if (playerData[0] == 1)
            {
                switch (newState)
                {
                    case Team.Defender:
                        Player1DefenderButton.SetIsOnWithoutNotify(true);
                        Player1HackerButton.SetIsOnWithoutNotify(false);
                        break;
                    case Team.Hacker:
                        Player1HackerButton.SetIsOnWithoutNotify(true);
                        Player1DefenderButton.SetIsOnWithoutNotify(false);
                        break;
                    case Team.None:
                        Player1HackerButton.SetIsOnWithoutNotify(false);
                        Player1DefenderButton.SetIsOnWithoutNotify(false); 
                        break;
                }  
            }
            
        }
    }
}