using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace com.Acinus.InTheDark
{
    public class LobbyTopPanel : MonoBehaviour
    {
        [Header("UI References")]
        public Text ConnectionStatusText;

        #region UNITY

        public void Update()
        {
            ConnectionStatusText.text = PhotonNetwork.NetworkClientState.ToString();
        }

        #endregion
    }
}