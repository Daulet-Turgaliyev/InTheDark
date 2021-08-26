using System;
using UnityEngine;

namespace com.Acinus.InTheDark
{
    [System.Serializable]
    public class PlayerParameters
    {
        [SerializeField] private float _healthPoints;
        [SerializeField] private float _fuel;
        
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _maxFuel;
        [SerializeField] private float _maxHealthPoints;

        public float GetHealthPoints => _healthPoints;
         public float GetMaxSpeed => _maxSpeed;

         public float GetFuel
         {
             get => _fuel;
             set => _fuel = value;
         }
         public float MaxHealthPoints => _maxHealthPoints;
         public float MaxFuel => _maxFuel;
    }
}