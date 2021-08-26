using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Acinus.InTheDark
{
    public class GameSettings : MonoBehaviour
    {

        [SerializeField] private int _monsterID;
        [SerializeField] private bool _isStart;

        private void Awake()
        {
            _monsterID = -1;
            _isStart = false;
            DontDestroyOnLoad(gameObject);
        }


        public int MonsterID
        {
            set
            {
                _monsterID = value;
            }
            get
            {
                return _monsterID;
            }
        }

        public bool isStart
        {
            set
            {
                _isStart = value;
            }
            get
            {
                return _isStart;
            }
        }

    }
}