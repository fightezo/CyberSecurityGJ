using GameModule.Class.Interface;
using PlayerModule.Class;
using PlayerModule.Class.Data;
using UnityEngine;

namespace GameModule.Class.Component
{
    internal class EndPanel : MonoBehaviour, IUIPanel
    {
        [SerializeField] private GameObject DefenderPanel;
        [SerializeField] private GameObject HackerPanel; 
        
        public GameObject GetSelf()
        {
            return gameObject;
        }

        public void EndPhaseUpdate()
        {
            
        }

        public void UpdateView(int securityLevel, int defenderSecurityThreshold, int hackerSecurityThreshold, PlayerManager localPlayerManager)
        {
            if (securityLevel >= defenderSecurityThreshold)
            {
                DefenderPanel.SetActive(localPlayerManager.GetTeam() == Team.Defender);
                HackerPanel.SetActive(localPlayerManager.GetTeam() == Team.Hacker);
                //In the DarkWeb; capturing hacker
                _DisplayDefenderEnding();
            }

            if (securityLevel <= hackerSecurityThreshold)
            {
                HackerPanel.SetActive(localPlayerManager.GetTeam() == Team.Hacker);
                DefenderPanel.SetActive(localPlayerManager.GetTeam() == Team.Defender);
                // In Hacked Computer; steals identity
                _DisplayHackerEnding();
            }

            // VictoryPanel.SetActive(localPlayerManager.GetState() == PlayerState.Invading);
            // GameOverPanel.SetActive(localPlayerManager.GetState() != PlayerState.Invading);
        }
        
        private void _DisplayHackerEnding()
        {
        }

        private void _DisplayDefenderEnding()
        {
        }

    }
}