using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
namespace com.Acinus.InTheDark
{
    public class ConnectingToServer : MonoBehaviourPunCallbacks
    {

        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        #region Callback

        public override void OnConnectedToMaster()
        {
            Debug.Log("Status: " + PhotonNetwork.NetworkClientState);
            PhotonNetwork.LoadLevel("Menu");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneManager.LoadScene(0);
            Debug.Log("ErrorConnect: " + cause.ToString());
        }

        public override void OnCustomAuthenticationFailed(string debugMessage)
        {
            SceneManager.LoadScene(0);
            Debug.Log("ErrorConnect: " + debugMessage.ToString());
        }
        #endregion

    }
}