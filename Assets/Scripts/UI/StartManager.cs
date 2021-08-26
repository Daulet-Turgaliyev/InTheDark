using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace com.Acinus.InTheDark
{
    public class StartManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Header("Player")]
        public GameObject playerPrefab;
        public GameObject GameSettingsPrefab;

        [Header("StartManager")]
        public GameObject StartButton;

        public Text CountPlayers_Text;
        public Text StartingTimer_Text;
        public Text RoomID_Text;

        public Button Start_Btn;
        public GameObject isToggle_Obj;
        public Toggle isToggle;
        /// Private
        /// 

        private GameObject PlayerObject;

        private GameObject _game_Settings_Object;
        private GameSettings _game_Settings;

        private float _timer = 3;

        public List<Player> playerListEntries;

        #region Unity

        private void Start()
        {
#if UNITY_EDITOR

            isToggle_Obj.SetActive(true);

#endif


            PhotonNetwork.AutomaticallySyncScene = true;

            if (GameObject.FindGameObjectWithTag("GameSettings") == null)
            {
                _game_Settings_Object = Instantiate(GameSettingsPrefab);
                _game_Settings = _game_Settings_Object.GetComponent<GameSettings>();
            }
            else
            {
                _game_Settings_Object = GameObject.FindGameObjectWithTag("GameSettings");
                _game_Settings = _game_Settings_Object.GetComponent<GameSettings>();
            }
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;

            if (playerListEntries == null)
            {
                playerListEntries = new List<Player>();
            }

            RoomID_Text.text = PlayerPrefs.GetString("RoomID");
            if (PhotonNetwork.IsMasterClient) { _game_Settings.isStart = false; }

            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                PlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(150, 154), playerPrefab.transform.rotation);
            }
            Update_PlayerCount();
        }

        private void FixedUpdate()
        {
            if (_timer > 0 & _game_Settings.isStart)
            {
                _timer -= Time.deltaTime;
                StartingTimer_Text.text = "Starting in " + Mathf.RoundToInt(_timer);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                StartButton.SetActive(true);
            }
            else
            {
                StartButton.SetActive(false);
            }
        }

        private void Update_PlayerCount()
        {
            playerListEntries.Clear();
            CountPlayers_Text.text = PhotonNetwork.PlayerList.Length.ToString() + "/" + PlayerPrefs.GetInt("MaxPlayers");
        }

        public void StatGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Start_Btn.interactable = false;
                _game_Settings.isStart = true;
                _timer = 3f;
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                if(isToggle_Obj.activeInHierarchy) {

                    if (isToggle.isOn)
                    {
                        _game_Settings.MonsterID = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)].ActorNumber;                    }
                    else
                    {
                        _game_Settings.MonsterID = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)].ActorNumber+1;
                    }
                }
                else
                {
                    _game_Settings.MonsterID = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)].ActorNumber;
                    Debug.Log(_game_Settings.MonsterID + " :: " + PhotonNetwork.PlayerList[0].ActorNumber);
                }    
            }
            StartCoroutine(Starting());
        }

        private IEnumerator Starting()
        {
            StartButton.SetActive(false);

            yield return new WaitForSeconds(3);

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Game");
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_game_Settings.MonsterID);
                stream.SendNext(_game_Settings.isStart);
            }
            else if (stream.IsReading)
            {
                _game_Settings.MonsterID = (int)stream.ReceiveNext();
                _game_Settings.isStart = (bool)stream.ReceiveNext();
            }
        }

        public void Leave()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Photon_CALLBACKS

        public override void OnLeftRoom()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            Destroy(_game_Settings_Object);
            SceneManager.LoadScene("Menu");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Update_PlayerCount();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Update_PlayerCount();
        }

        #endregion
    }
}