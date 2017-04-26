using System.Collections.Generic;
using FPS.EventSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{

    public class GlobalPlayerManager : NetworkBehaviour
    {

        public static GlobalPlayerManager Instance;

        private GameNetworkManager _theNetworkManager;

        private GameNetworkManager TheNetworkManager
        {
            get
            {
                if (_theNetworkManager == null)
                {
                    _theNetworkManager = FindObjectOfType<GameNetworkManager>();
                }
                return _theNetworkManager;
            }
        }

        private NetworkClient _client;
        private NetworkClient Client
        {
            get
            {
                if (_client == null)
                {
                    if (TheNetworkManager != null) { _client = TheNetworkManager.client; }
                    else { Debug.LogWarning("Failed to find NetworkManager to set the client."); }
                }
                return _client;
            }
        }

        protected void Awake()
        {
            Instance = this;
        }

        public override void OnStartClient()
        {
            Client.RegisterHandler(4000, OnPlayerListRequest);
            Client.RegisterHandler(4001, OnPlayerUpdateRequest);
        }

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler(4000, OnPlayerListRequest);
            NetworkServer.RegisterHandler(4001, OnPlayerUpdateRequest);
        }

        #region  Network Handlers

        private void OnPlayerUpdateRequest(NetworkMessage netMsg)
        {
            NetworkMessagePlayerUpdate msg = netMsg.ReadMessage<NetworkMessagePlayerUpdate>();

            if (isServer && msg.isServerResponse == false)
            {
                msg.isServerResponse = true;
                
                NetworkInstanceId netID = new NetworkInstanceId(msg.PlayerNetID);
                GameObject go = NetworkServer.FindLocalObject(netID);
                if (go != null)
                {
                    PlayerInfo pinfo = go.GetComponent<PlayerInfo>();
                    msg.deaths = pinfo.ThePlayerData.playerDeaths;
                    msg.kills = pinfo.ThePlayerData.playerKills;
                    NetworkServer.SendToAll(4001, msg);
                }

            }
            else if (Client.isConnected)
            {
                NetworkInstanceId netID = new NetworkInstanceId(msg.PlayerNetID);
                GameObject go = ClientScene.FindLocalObject(netID);
                if (go != null)
                {
                    PlayerInfo pinfo = go.GetComponent<PlayerInfo>();
                    pinfo.ThePlayerData.playerKills = msg.kills;
                    pinfo.ThePlayerData.playerDeaths = msg.deaths;
                }
            }
        }

        private void OnPlayerListRequest(NetworkMessage netMsg)
        {
            NetworkMessagePlayersList msg = netMsg.ReadMessage<NetworkMessagePlayersList>();

            if (isServer && msg.isServerResponse == false)
            {
                msg.isServerResponse = true;

                List<uint> plist = new List<uint>();

                for (int i = 0; i < TheNetworkManager.ConnectedPlayers.Count; i++)
                {
                    uint player_net_id = TheNetworkManager.ConnectedPlayers[i]
                        .GetComponent<NetworkIdentity>()
                        .netId.Value;

                    plist.Add(player_net_id);
                }

                msg.NetIDs = plist.ToArray();

                NetworkServer.SendToClient(netMsg.conn.connectionId, 4000, msg);
                //NetworkServer.SendToAll(4000, msg);
                //NetworkInstanceId netID = new NetworkInstanceId(msg.NetID);
                //GameObject go = NetworkServer.FindLocalObject(netID);
                //if (go != null)
                //{
                //}
            }
            else if (Client.isConnected)
            {
                List<GameObject> players = new List<GameObject>();
                for (int i = 0; i < msg.NetIDs.Length; i++)
                {
                    NetworkInstanceId netID = new NetworkInstanceId(msg.NetIDs[i]);
                    GameObject go = ClientScene.FindLocalObject(netID);
                    if (go != null)
                    {
                        players.Add(go);   
                    }
                }

                EventMessenger.Instance.Raise(new EventReceivedPlayersList(players));
            }
        }


        #endregion  Network Handlers

        #region Public Methods

        public void RequestPlayersList()
        {
            NetworkMessagePlayersList msg = new NetworkMessagePlayersList();
            msg.isServerResponse = false;
            Client.Send(4000, msg);
        }

        public void RequestPlayerUpdate(PlayerInfo pinfo, uint playerNetID)
        {
            NetworkMessagePlayerUpdate msg = new NetworkMessagePlayerUpdate();

            msg.isServerResponse = false;
            msg.PlayerNetID = playerNetID;
            msg.kills = pinfo.ThePlayerData.playerKills;
            msg.deaths = pinfo.ThePlayerData.playerDeaths;

            Client.Send(4001, msg);
        }

        #endregion

        public class NetworkMessagePlayersList : MessageBase
        {
            public bool isServerResponse = false;
            public uint[] NetIDs;
        }

        public class NetworkMessagePlayerUpdate : MessageBase
        {
            public bool isServerResponse = false;
            public uint PlayerNetID;
            public int kills;
            public int deaths;

            public string playerTag;
            public string playerUUID;
        }
    }
}