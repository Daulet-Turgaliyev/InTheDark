using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Acinus.InTheDark
{
    public class PlayerAnimation : MonoBehaviour
    {
        public Animator animator;
        public int animationIndex;

        public HudController hudController;
        
        private PlayerController _playerController;

        private float _speed;
        private bool _isLocal;
        public void Start()
        {
            _playerController = GetComponent<PlayerController>();
            
            _isLocal = _playerController.IsLocal;
            
            _speed = ((_playerController.GetCurrentSpeed/2)/10)-3.5f;
        }

        private void FixedUpdate()
        {
            if (_isLocal)
            {
                Animator_Controller_Me();
            }
            else
            {
                Animator_Controller();
            }
        }

        public void Animator_Controller_Me()
        {
            if (animationIndex != 7)
            {
                if (Math.Abs(hudController.VerticalVector) > Math.Abs(hudController.HorizontalVector)) // Определяем направление
                {
                    _playerController.Avatar_Flip(false);
                    if (_playerController.GetCurrentSpeed > _speed) // Определяем скорость
                    {
                        if (hudController.VerticalVector > .5f) { animator.Play("run_up"); animationIndex = 3; }
                        else { animator.Play("run_down"); animationIndex = 4; }
                    }
                    else
                    {
                        if (hudController.VerticalVector > .1f) { animator.Play("walk_up"); animationIndex = 5; }
                        else { animator.Play("walk_down"); animationIndex = 6; }
                    }
                }
                else if (Math.Abs(hudController.VerticalVector) < Math.Abs(hudController.HorizontalVector))
                {
                    if (_playerController.GetCurrentSpeed > _speed)
                    {
                        if (hudController.HorizontalVector < .5f) { animator.Play("run_side"); _playerController.Avatar_Flip(false); animationIndex = 2; }
                        else { animator.Play("run_side"); _playerController.Avatar_Flip(true); animationIndex = 2; }
                    }
                    else
                    {
                        if (hudController.HorizontalVector < .1f) { animator.Play("walk_side"); _playerController.Avatar_Flip(false); animationIndex = 1; }
                        else { animator.Play("walk_side"); _playerController.Avatar_Flip(true); animationIndex = 1; }
                    }
                }
                else { animator.Play("idle_side"); animationIndex = 0; }
            }
        }

        public void Animator_Controller()
        {
            if (animationIndex != 7)
            {
                switch (animationIndex)
                {
                    case 0:
                        animator.Play("idle_side");
                        break;
                    case 1:
                        animator.Play("walk_side");
                        break;
                    case 2:
                        animator.Play("run_side");
                        break;
                    case 3:
                        animator.Play("run_up");
                        break;
                    case 4:
                        animator.Play("run_down");
                        break;
                    case 5:
                        animator.Play("walk_up");
                        break;
                    case 6:
                        animator.Play("walk_down");
                        break;
                }
            }
        }
    }
}