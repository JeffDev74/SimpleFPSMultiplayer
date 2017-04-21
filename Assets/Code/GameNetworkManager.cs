using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class GameNetworkManager : NetworkManager
	{
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            string playerUUID = extraMessageReader.ReadString();
            
            GameObject instantiatedPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            PlayerInfo pInfo = instantiatedPlayer.GetComponent<PlayerInfo>();
            if(pInfo != null)
            {
                pInfo.ThePlayerData = new PlayerData();
                pInfo.ThePlayerData.playerUUID = playerUUID;
            }

            NetworkServer.AddPlayerForConnection(conn, instantiatedPlayer, playerControllerId);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            OnServerAddPlayer(conn, playerControllerId, null);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            ClientScene.Ready(conn);
            NetworkPlayerStart msg = new NetworkPlayerStart();
            msg.PlayerUUID = System.Guid.NewGuid().ToString();
            ClientScene.AddPlayer(conn, 0, msg);
        }

        public class NetworkPlayerStart : MessageBase
        {
            public string PlayerUUID;
        }
    }
}