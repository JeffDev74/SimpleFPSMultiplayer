﻿using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
	public class PlayerInfo : NetworkBehaviour
	{
        public PlayerData _thePlayerData;
        public PlayerData ThePlayerData
        {
            get { return _thePlayerData; }
            set { _thePlayerData = value; }
        }

        public Transform _playerHud;
        public Transform PlayerHud
        {
            get { return _playerHud; }
            set { _playerHud = value; }
        }

        private Text _thePlayerTagText;
        private Text ThePlayerTagText
        {
            get
            {
                if(_thePlayerTagText == null)
                {
                    _thePlayerTagText = GetComponentInChildren<Text>();
                }
                return _thePlayerTagText;
            }
        }

        [SyncVar]
        public string PlayerTag;

        public void SetupTag()
        {
            ThePlayerTagText.text = PlayerTag;
        }
    }
}