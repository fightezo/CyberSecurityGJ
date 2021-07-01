using GameModule.Class.Interface;
using PlayerModule.Class;
using PlayerModule.Class.Data;
using UnityEngine;

namespace GameModule.Class.Component
{

    internal class EndPanel : MonoBehaviour, IUIPanel
    {
        [SerializeField] private GameObject VictoryPanel;
        [SerializeField] private GameObject GameOverPanel; 
        
        public GameObject GetSelf()
        {
            return gameObject;
        }

        public void UpdateView(int securityLevel, int citizenSecurityThreshold, int hackerSecurityThreshold, PlayerManager localPlayerManager)
        {
            if (securityLevel >= citizenSecurityThreshold)
            {
                VictoryPanel.SetActive(localPlayerManager.GetTeam() == Team.Citizen);
                GameOverPanel.SetActive(localPlayerManager.GetTeam() == Team.Hacker);
                //In the DarkWeb; capturing hacker
                // _DisplayCitizenEnding();
            }

            if (securityLevel <= hackerSecurityThreshold)
            {
                VictoryPanel.SetActive(localPlayerManager.GetTeam() == Team.Hacker);
                GameOverPanel.SetActive(localPlayerManager.GetTeam() == Team.Citizen);
                // In Hacked Computer; steals identity
                // _DisplayHackerEnding();
            }

            // VictoryPanel.SetActive(localPlayerManager.GetState() == PlayerState.Invading);
            // GameOverPanel.SetActive(localPlayerManager.GetState() != PlayerState.Invading);
        }
        
        private void _DisplayHackerEnding()
        {
        }

        private void _DisplayCitizenEnding()
        {
        }

    }
}