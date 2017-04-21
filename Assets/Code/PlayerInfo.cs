using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class PlayerInfo : NetworkBehaviour
	{
        PlayerData _thePlayerData;
        PlayerData ThePlayerData
        {
            get { return _thePlayerData; }
            set { _thePlayerData = value; }
        }
	}
}