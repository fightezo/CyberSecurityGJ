using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class.Component.UI
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        const string playerNamePrefKey = "PlayerName";

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            var defaultName = string.Empty;
            var inputField = GetComponent<InputField>();
            if (inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion


        #region Public Methods
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        #endregion
    }
}