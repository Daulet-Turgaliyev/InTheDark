using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace com.Acinus.InTheDark
{
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        public PlayerParameters playerParameters;

        [Header("Components")]
        public PhotonView photonView;
        public PlayerAnimation playerAnimation;
        public HudController hudController;
        public Rigidbody2D rigidbody2D;
        
        [Header("Player Object")]
        public GameObject hudControllerObject;
        public GameObject mainCamera;
        public GameObject localLightObject;
        public TextMeshProUGUI nickNameText;
        
        public SpriteRenderer avatar;

        public bool IsLocal { private set; get; }

        public float GetCurrentSpeed => Math.Abs(rigidbody2D.velocity.x) + Math.Abs(rigidbody2D.velocity.y);

        private void Start()
        {

            nickNameText.SetText(photonView.Owner.NickName);
            IsLocal = photonView.IsMine;
            if (IsLocal)
            {
                nickNameText.color = Color.green;
                mainCamera.SetActive(true);
            }
            else
            {
                Destroy(mainCamera);
                Destroy(hudControllerObject);
                Destroy(localLightObject);
            }
        }



        private void FixedUpdate()
        {
            if (IsLocal)
            {
                ProcessInputs();
            }
        }

        private void ProcessInputs()
        {
            rigidbody2D.AddForce(new Vector2
                (hudController.HorizontalVector,
                 hudController.VerticalVector)
                  * playerParameters.GetMaxSpeed);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                stream.SendNext(avatar.flipX);
                stream.SendNext(playerAnimation.animationIndex);
            }
            else if (stream.IsReading)
            {
                avatar.flipX = (bool)stream.ReceiveNext();
                playerAnimation.animationIndex = (int)stream.ReceiveNext();
            }

        }

        public void Avatar_Flip(bool flipX)
        {
            avatar.flipX = flipX;
        }

        private void OnApplicationQuit()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        public void OnClickLeftRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        public override void OnLeftRoom()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            SceneManager.LoadScene("Menu");
        }



    }


}