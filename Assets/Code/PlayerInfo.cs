using UnityEngine.Networking;

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
	}
}