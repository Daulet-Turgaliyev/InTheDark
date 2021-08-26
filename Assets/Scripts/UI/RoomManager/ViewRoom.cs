using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace com.Acinus.InTheDark
{
    public class ViewRoom : MonoBehaviourPunCallbacks
    {
        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        private Dictionary<int, GameObject> playerListEntries;

        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;

        public GameObject Loading;
        public GameObject Empty_List;

        public void Awake()
        {

            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();

        }

        public void OnClickJoinLobby()
        {
            PhotonNetwork.JoinLobby();
        }
        public void OnClickLeftLobby()
        {
            PhotonNetwork.LeaveLobby();
        }

        private void Start()
        {

        }
        public void OnRoomRandom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {

            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList || info.MaxPlayers == info.PlayerCount)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }

        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.transform.position = new Vector3(0, 0, -181);
                entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

                roomListEntries.Add(info.Name, entry);
            }

            if (RoomListContent.transform.childCount == 0) { Empty_List.SetActive(true); }
            else { Empty_List.SetActive(false); }
        }

        private void ClearRoomListView()
        {
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }

        public void JoinRandomRoom()
        {
            Loading.SetActive(true);
            PhotonNetwork.JoinRandomRoom();
        }

        public void StartLoading()
        {
            Loading.SetActive(true);
        }

        #region PUN CALLBACKS
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Loading.SetActive(false);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Room");
        }


        public override void OnJoinedLobby()
        {

        }

        #endregion

    }
}