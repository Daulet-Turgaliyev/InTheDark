using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace com.Acinus.InTheDark
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {
        public GameObject ShowRoom;
        public GameObject CreateRoom;

        public GameObject Welcome;
        public InputField Input_NickName;
        public Button NextBtn;

        public Text TextVersion;

        public void ActiveShowRoom(bool act) { ShowRoom.SetActive(act); }
        public void ActiveCreateRoom(bool act) { CreateRoom.SetActive(act); }

        private void Start()
        {
            TextVersion.text = Application.version;
            OnWelcome();
        }

        private void OnWelcome()
        {
            NextBtn.interactable = false;

            if (PlayerPrefs.GetString("NickName").Length < 3)
            {
                Welcome.SetActive(true);

                NextBtn.interactable = false;
                PlayerPrefs.SetString("NickName", PhotonNetwork.LocalPlayer.NickName);
                PlayerPrefs.Save();
            }
            else
            {
                PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("NickName");
                Debug.Log(PhotonNetwork.LocalPlayer.NickName);

                Destroy(Welcome);
            }
        }

        public void SaveName()
        {
            PhotonNetwork.LocalPlayer.NickName = Input_NickName.text;
            PlayerPrefs.SetString("NickName", PhotonNetwork.LocalPlayer.NickName);
            PlayerPrefs.Save();

            Destroy(Welcome);
        }

        public void Input_Update()
        {
            if (Input_NickName.text.Length < 3)
            {
                NextBtn.interactable = false;
            }
            else
            {
                NextBtn.interactable = true;
            }
        }

        #region Callback


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