using System;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace com.Acinus.InTheDark
{
    public class LightController : MonoBehaviour, IPunObservable
    {
        public GameObject localLightObject;

        public PlayerParameters playerParameters;
        public HudController hudController;

        public Light2D light2D;
        public CircleCollider2D circleDamageCollider2D;

        private bool isLocal;

        [SerializeField] private float _consumption;


        private void Start()
        {
            var playerController = GetComponentInParent<PlayerController>();

            if (playerController != null)
            {
                isLocal = playerController.IsLocal;
                playerParameters = playerController.playerParameters;
            }
            else
            {
                Debug.LogError("playerController not found");
            }
        }

        void FixedUpdate()
        {
            if (isLocal)
            {
                if (playerParameters.GetFuel <= 0)
                {
                    light2D.pointLightOuterRadius = 0f;
                    playerParameters.GetFuel = 0;
                }
                else
                {
                    playerParameters.GetFuel -= hudController.LightRadius / _consumption;
                    light2D.pointLightOuterRadius = hudController.LightRadius * 2f;
                    circleDamageCollider2D.radius = light2D.pointLightOuterRadius * 2;
                }

                if (light2D.pointLightOuterRadius > 1.85f)// Переделай колхоз
                {
                    localLightObject.SetActive(false);
                }
                else
                {
                    localLightObject.SetActive(true);
                }
            }
            else// Переделай колхоз
            {
                if (circleDamageCollider2D.isActiveAndEnabled)
                {
                    circleDamageCollider2D.radius = light2D.pointLightOuterRadius * 2;
                }
            }

            if(light2D.pointLightOuterRadius < .2f) // Переделай колхоз
            {
                circleDamageCollider2D.enabled = false;
            }
            else
            {
                circleDamageCollider2D.enabled = true;

            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                stream.SendNext(light2D.pointLightOuterRadius);
            }
            else if (stream.IsReading)
            {
                light2D.pointLightOuterRadius = (float)stream.ReceiveNext();
            }

        }
    }
}