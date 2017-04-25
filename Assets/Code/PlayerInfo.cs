using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
	public class PlayerInfo : NetworkBehaviour
	{
        public PlayerData _thePlayerData;
        public PlayerData ThePlayerData
        {
            get
            {
                if(_thePlayerData == null)
                {
                    _thePlayerData = new PlayerData();
                }
                return _thePlayerData;
            }
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

        public void SetupTag(string tag)
        {
            ThePlayerTagText.text = tag;
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(ThePlayerTagText.text))
            {
                ThePlayerTagText.text = ThePlayerData.playerTag;
            }
        }

        public override bool OnSerialize(NetworkWriter writer, bool initialState)
        {
            if(initialState)
            {
                writer.Write(ThePlayerData.playerUUID);
                writer.Write(ThePlayerData.playerTag);
                writer.Write(ThePlayerData.PlayerNetID);

                return true;
            }
            return false;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            if(initialState)
            {
                ThePlayerData.playerUUID = reader.ReadString();
                ThePlayerData.playerTag = reader.ReadString();
                ThePlayerData.PlayerNetID = reader.ReadUInt32();
                SetupTag(ThePlayerData.playerTag);
            }
        }
    }
}