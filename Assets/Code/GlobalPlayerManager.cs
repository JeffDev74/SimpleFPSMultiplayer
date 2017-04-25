using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{

    public class GlobalPlayerManager : NetworkBehaviour
    {

        public static GlobalPlayerManager Instance;

        private NetworkClient _client;
        private NetworkClient Client
        {
            get
            {
                if (_client == null)
                {
                    NetworkManager net = FindObjectOfType<NetworkManager>();
                    if (net != null) { _client = net.client; }
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
        }

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler(4000, OnPlayerListRequest);
        }

        private void OnPlayerListRequest(NetworkMessage netMsg)
        {
            NetworkMessagePlayersList msg = netMsg.ReadMessage<NetworkMessagePlayersList>();

            if (isServer && msg.isServerResponse == false)
            {
                msg.isServerResponse = true;

                Debug.Log("SERVER received request for players list");
                NetworkServer.SendToClient(netMsg.conn.connectionId, 4000, msg);

                //NetworkInstanceId netID = new NetworkInstanceId(msg.NetID);
                //GameObject go = NetworkServer.FindLocalObject(netID);
                //if (go != null)
                //{

                //}
            }
            else if (Client.isConnected)
            {
                Debug.Log("CLIENT receivec the updated players list");
                //NetworkInstanceId netID = new NetworkInstanceId(msg.NetID);
                //GameObject go = ClientScene.FindLocalObject(netID);
                //if (go != null)
                //{
                //}
            }
        }

        public void RequestPlayersList()
        {
            NetworkMessagePlayersList msg = new NetworkMessagePlayersList();
            msg.isServerResponse = false;
            Client.Send(4000, msg);
        }

        public class NetworkMessagePlayersList : MessageBase
        {
            public bool isServerResponse = false;
            public uint[] NetIDs;
        }
    }
}