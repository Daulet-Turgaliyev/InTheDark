using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace com.Acinus.InTheDark
{
    public class RoomListEntry : MonoBehaviour
    {
        public Text RoomNameText;
        public Text RoomPlayersText;
        public Button JoinRoomButton;

        private string roomName;
        private GameObject _viewRoom;

        public void Start()
        {
            _viewRoom = GameObject.FindGameObjectWithTag("ViewRoom");

            JoinRoomButton.onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }

                if (_viewRoom != null)
                {
                    _viewRoom.GetComponent<ViewRoom>().StartLoading();
                }

                PhotonNetwork.JoinRoom(roomName);
            });
        }



        public void Initialize(string name, byte currentPlayers, byte maxPlayers)
        {
            roomName = name;


            PlayerPrefs.SetString("RoomID", name);
            PlayerPrefs.SetInt("MaxPlayers", maxPlayers);
            PlayerPrefs.Save();

            RoomNameText.text = name;
            RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
        }
    }
}