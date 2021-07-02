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

        public void OnStartGameButtonClicked()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (GameManager.Instance.GetPlayersTeamStateList().Contains(Team.None)) return;
            GameManager.Instance.UpdateNextGameState();
        }
        
        public void OnDefenderButtonClicked()
        {
            if (GameManager.Instance.GetPlayersTeamStateList().Contains(Team.Defender) &&
                GameManager.Instance.GetPlayersTeamStateList().FindIndex(x => x == Team.Defender) != GameManager.Instance.GetLocalPlayerIndex())
                return;
            
            var newState = GameManager.Instance.GetPlayersTeamStateList()[GameManager.Instance.GetLocalPlayerIndex()];
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

            var itemList = new[] {GameManager.Instance.GetLocalPlayerIndex(), (int) newState};
            GameManager.Instance.photonView.RPC("_RPC_SendDefenderButtonClicked", RpcTarget.AllBuffered, itemList);
        }

        public void OnHackerButtonClicked()
        {
            if (GameManager.Instance.GetPlayersTeamStateList().Contains(Team.Hacker) &&
                GameManager.Instance.GetPlayersTeamStateList().FindIndex(x => x == Team.Hacker) != GameManager.Instance.GetLocalPlayerIndex())
                return;

            var newState = GameManager.Instance.GetPlayersTeamStateList()[GameManager.Instance.GetLocalPlayerIndex()];
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

            var itemList = new[] {GameManager.Instance.GetLocalPlayerIndex(), (int) newState};
            GameManager.Instance.photonView.RPC("_RPC_SendHackerButtonClicked", RpcTarget.AllBuffered, itemList);
        }

        public GameObject GetSelf()
        {
            return gameObject;
        }
        
        
    }
}