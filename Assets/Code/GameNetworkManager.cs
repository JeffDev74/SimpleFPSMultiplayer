using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class GameNetworkManager : NetworkManager
	{
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            string playerUUID = extraMessageReader.ReadString();
            string playerTag = extraMessageReader.ReadString();
            
            GameObject instantiatedPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            PlayerInfo pInfo = instantiatedPlayer.GetComponent<PlayerInfo>();
            if(pInfo != null)
            {
                pInfo.ThePlayerData = new PlayerData();
                pInfo.ThePlayerData.playerUUID = playerUUID;
                pInfo.ThePlayerData.playerTag = playerTag;
                pInfo.PlayerTag = playerTag;
                pInfo.SetupTag();
            }

            NetworkServer.AddPlayerForConnection(conn, instantiatedPlayer, playerControllerId);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            OnServerAddPlayer(conn, playerControllerId, null);
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            #region ex
            // If you don't override this method unity calls the example code below here...
            // This is why the client ready was being called twice

            /*
            ClientScene.Ready(conn);
 
            if (!this.m_AutoCreatePlayer) return;
 
            bool flag1 = ClientScene.localPlayers.Count == 0;
            bool flag2 = false;
 
            for (int index = 0; index < ClientScene.localPlayers.Count; ++index)
            {
                if ((UnityEngine.Object)ClientScene.localPlayers[index].gameObject != (UnityEngine.Object)null)
                {
                    flag2 = true;
                    break;
                }
            }
 
            if (!flag2) flag1 = true;
            if (!flag1) return;
 
            ClientScene.AddPlayer((short)0);
            */
            #endregion ex
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            ClientScene.Ready(conn);
            NetworkPlayerStart msg = new NetworkPlayerStart();
            msg.PlayerUUID = System.Guid.NewGuid().ToString();
            msg.PlayerTag  = playerTag;
            ClientScene.AddPlayer(conn, 0, msg);
        }

        public class NetworkPlayerStart : MessageBase
        {
            public string PlayerUUID;
            public string PlayerTag;
        }

        string playerTag = "Unknown";
        public void GameStartClient(string playerTag)
        {
            this.playerTag = playerTag;
            StartClient();
        }
    }
}