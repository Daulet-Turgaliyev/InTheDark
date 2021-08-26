using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.Acinus.InTheDark
{
    public class HudController : MonoBehaviour
    {
        [Header("Joystick")]
        public Slider lightController;
        public Joystick joystickController;
        [Header("Fuel")]
        public Image fuelImage;
        public Text fuelText;
        [Header("Health")]
        public Text healthPointsText;
        public Image healthPointsImage;

        private PlayerParameters _playerParameters;
        
        public float HorizontalVector => joystickController.Horizontal;

        public float VerticalVector => joystickController.Vertical;

        public float LightRadius => lightController.value;

        
        
        private void Start()
        {
            _playerParameters = GetComponentInParent<PlayerController>().playerParameters;
        }

        public void UpdateUserInterface()
        {
            healthPointsText.text = "HP: " + Math.Round(_playerParameters.GetHealthPoints);
            fuelText.text = "Fuel: " + Math.Round(_playerParameters.GetFuel, 1);

            fuelImage.fillAmount = _playerParameters.GetFuel / _playerParameters.MaxFuel;
            healthPointsImage.fillAmount = _playerParameters.GetHealthPoints / _playerParameters.MaxHealthPoints;
        }
    }
}