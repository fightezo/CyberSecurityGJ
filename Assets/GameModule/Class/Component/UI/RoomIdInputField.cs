using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class.Component.UI
{
    public class RoomIdInputField : MonoBehaviour
    {
        private string _roomId;
        private void Start()
        {
            var inputField = GetComponent<InputField>();
            if (inputField != null)
            {
                inputField.text = string.Empty;
            }
        }

        #region Public Methods
        public void SetRoomId(string roomId)
        {
            _roomId = roomId;
        }

        public string GetRoomId()
        {
            return _roomId; 
        }
        #endregion
    }
}