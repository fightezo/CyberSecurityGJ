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
        
        public void OnCitizenButtonClicked()
        {
            if (GameManager.Instance.GetPlayersTeamStateList().Contains(Team.Citizen) &&
                GameManager.Instance.GetPlayersTeamStateList().FindIndex(x => x == Team.Citizen) != GameManager.Instance.GetLocalPlayerIndex())
                return;
            
            var newState = GameManager.Instance.GetPlayersTeamStateList()[GameManager.Instance.GetLocalPlayerIndex()];
            switch (newState)
            {
                case Team.None:
                case Team.Hacker:
                    newState = Team.Citizen;
                    break;
                case Team.Citizen:
                    newState = Team.None;
                    break;
            }

            var itemList = new[] {GameManager.Instance.GetLocalPlayerIndex(), (int) newState};
            GameManager.Instance.photonView.RPC("_RPC_SendCitizenButtonClicked", RpcTarget.AllBuffered, itemList);
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
                case Team.Citizen:
                    newState = Team.Hacker;
                    break;
                case Team.Hacker:
                    newState = Team.None;
                    break;
            }

            var itemList = new[] {GameManager.Instance.GetLocalPlayerIndex(), (int) newState};
            GameManager.Instance.photonView.RPC("_RPC_SendHackerButtonClicked", RpcTarget.AllBuffered, itemList);
        }
 
        //
        // [PunRPC]
        // private void _RPC_SendCitizenButtonClicked(int[] newState)
        // {
        //     GameManager.Instance.GetPlayersTeamStateList()[newState[0]] = (Team) newState[1];
        //     Debug.Log(
        //         $"_RPC_SendCitizenButtonClicked:: {GameManager.Instance.GetPlayersList()[newState[0]]}::{GameManager.Instance.GetPlayersTeamStateList()[newState[0]]}");
        // }
        //
        // [PunRPC]
        // private void _RPC_SendHackerButtonClicked(int[] newState)
        // {
        //     GameManager.Instance.GetPlayersTeamStateList()[newState[0]] = (Team) newState[1];
        //     Debug.Log(
        //         $"_RPC_SendHackerButtonClicked:: {GameManager.Instance.GetPlayersList()[newState[0]]}::{GameManager.Instance.GetPlayersTeamStateList()[newState[0]]}");
        // }
        //
        //
        public GameObject GetSelf()
        {
            return gameObject;
        }
        
        
    }
}