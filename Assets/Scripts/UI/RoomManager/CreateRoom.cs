using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace com.Acinus.InTheDark
{
    public class CreateRoom : MonoBehaviourPunCallbacks
    {
        public Slider MaxPlayersSlider;
        public Text MaxPlayersTexr;

        public GameObject Loading;

        private readonly char[] _chars = "QWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray();
        private string RoomID;
        private string Generating_RoomID
        {
            get
            {
                string id = "";
                int count = Random.Range(4, 7);

                for (int i = 0; i < count; i++)
                {
                    id += _chars[Random.Range(0, _chars.Length)];
                }
                return id;
            }
        }

        public void Update_TextSlider()
        {
            MaxPlayersTexr.text = "Max Players: " + MaxPlayersSlider.value;
        }

        public void OnCreateRoomButton()
        {
            Loading.SetActive(true);
            int maxPlayers = (int)MaxPlayersSlider.value;
            RoomID = Generating_RoomID;

            PlayerPrefs.SetString("RoomID", RoomID);
            PlayerPrefs.SetInt("MaxPlayers", maxPlayers);
            PlayerPrefs.Save();
            RoomOptions options = new RoomOptions { MaxPlayers = (byte)maxPlayers };
            PhotonNetwork.CreateRoom(RoomID, options, null);

        }

        public override void OnCreatedRoom()
        {
            // PhotonNetwork.LoadLevel("Room");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Loading.SetActive(false);
            Debug.Log(message);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            PhotonNetwork.LoadLevel(0);
        }

    }

}