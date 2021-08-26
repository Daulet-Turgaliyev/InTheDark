using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.Acinus.InTheDark
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        private Dictionary<string, Player> cachedPlayerList;

        public PhotonView pv;

        public GameObject WinOrLose_Object;
        public Text WinOrLose_Text;

        private GameObject _monster;
        private GameObject[] _players;

        [Header("Prefabs")]
        public GameObject playerPrefab;
        public GameObject monsterPrefab;
        public GameObject spectorPrefab;

        private GameSettings _gameSettings;

        private int _countingscene;// Костыль, пересмотри

        #region Triggers

        public override void OnJoinedRoom()
        {
            Debug.Log("I'm Joind Room ");
        }

        public override void OnLeftRoom()
        {

            pv.RPC("Counting_Dead_Players", RpcTarget.All);

            PhotonNetwork.AutomaticallySyncScene = false;
            SceneManager.LoadScene("Menu");
        }

        #endregion

        #region Unity
        public void Awake()
        {
            cachedPlayerList = new Dictionary<string, Player>();
        }

        public void Start()
        {
            _countingscene = 0;

            PhotonNetwork.AutomaticallySyncScene = true;

            _gameSettings = GameObject.FindGameObjectWithTag("GameSettings").GetComponent<GameSettings>();

            if (_gameSettings.MonsterID == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                PhotonNetwork.Instantiate(monsterPrefab.name, new Vector2(24, -31), monsterPrefab.transform.rotation);
            }
            else
            {
                if (PhotonNetwork.PlayerList.Length > 4) {

                    if(_gameSettings.MonsterID == 0) { _gameSettings.MonsterID += 2; }

                    if (_gameSettings.MonsterID - 1 == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        PhotonNetwork.Instantiate(monsterPrefab.name, new Vector2(24, -31), monsterPrefab.transform.rotation);
                    }
                    else
                    {
                        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(7, -2), playerPrefab.transform.rotation);
                    }
                }
                else
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(7, -2), playerPrefab.transform.rotation);
                }
            }

            _gameSettings.MonsterID = -1;
            _gameSettings.isStart = false;
        }

        public void SpawnGhost(Transform player_position, bool isMine, bool isMonster)
        {
            if (!isMonster & isMine) { Instantiate(spectorPrefab, player_position.transform.position, player_position.transform.rotation); }
        }


        public IEnumerator Update_Counting()
        {
            yield return new WaitForSeconds(2);
            pv.RPC("Counting_Dead_Players", RpcTarget.All);
        }

        [PunRPC]private void Counting_Dead_Players()
        {
            _monster = GameObject.FindGameObjectWithTag("MonsterBody");
            Debug.Log(_monster);
            if (_monster == null)
            {
                WinOrLose("Players ");
            }
            _players = GameObject.FindGameObjectsWithTag("PlayerBody");
  
            Debug.Log(_players.Length);
            if (_players.Length == 0) { WinOrLose("Monster "); }
        }

        public void WinOrLose(string winner)
        {
            WinOrLose_Object.SetActive(true);
            WinOrLose_Text.text = winner + " won";
            StartCoroutine(Timer_EndGame());
        }

        IEnumerator Timer_EndGame()
        {
            yield return new WaitForSeconds(3);
            if (PhotonNetwork.IsMasterClient & _countingscene == 0)
            {
                PhotonNetwork.DestroyAll();
                PhotonNetwork.LoadLevel("Room");
            }
            _countingscene++;
        }

        public void Leave()
        {
            PhotonNetwork.LeaveRoom();
        }

        public int Players_Count()
        {
            _players = GameObject.FindGameObjectsWithTag("PlayerBody");

            return _players.Length;
        }


        #endregion
    }
}