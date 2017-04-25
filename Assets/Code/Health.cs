using FPS.EventSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class Health : NetworkBehaviour
	{
        private Transform _theTransform;
        private Transform TheTransform
        {
            get
            {
                if(_theTransform == null)
                {
                    _theTransform = transform;
                }
                return _theTransform;
            }
        }

        public const int maxHealth = 100;
        public bool destroyOnDeath;

        private PlayerInfo _thePlayerInfo;
        private PlayerInfo ThePlayerInfo
        {
            get
            {
                if(_thePlayerInfo == null)
                {
                    _thePlayerInfo = GetComponent<PlayerInfo>();
                }
                return _thePlayerInfo;
            }
        }

        private NetworkStartPosition[] spawnPoints;

        public RectTransform healthBar;

        [SyncVar(hook = "OnChangeHealth")]
        public int currentHealth = maxHealth;

        void OnChangeHealth(int health)
        {
            currentHealth = health;
            healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        }

        private void OnEnable()
        {
            EventMessenger.Instance.AddListner<EventPlayerRespawn>(OnPlayerRespawn);
        }

        private void OnDisable()
        {
            EventMessenger.Instance.RemoveListner<EventPlayerRespawn>(OnPlayerRespawn);
        }

        private void OnPlayerRespawn(EventPlayerRespawn e)
        {
            RespawnPlayerAtRandom();
        }

        private void Start()
        {
            if(isLocalPlayer)
            {
                spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            }
        }

        public void TakeDamage(int amount, string senderPlayerUUID, string receiverPlayerUUID)
        {
            if(isServer == false)
            {
                return;
            }

            currentHealth -= amount;
            if(currentHealth <= 0)
            {
                if(destroyOnDeath)
                {
                    Destroy(gameObject);
                }
                else
                {
                    CmdUpdatePlayerStats(senderPlayerUUID, receiverPlayerUUID);

                    currentHealth = maxHealth;

                    // called on the server but invoked on the clients
                    RpcRespawn();
                }
            }
        }

        [Command]
        private void CmdUpdatePlayerStats(string SenderPlayerUUID, string ReceiverPlayerUUID)
        {
            GameNetworkManager GMManager = FindObjectOfType<GameNetworkManager>();
            if(GMManager != null)
            {
                GameObject senderGO = GMManager.GetPlayer(SenderPlayerUUID);
                GameObject receiverGO = GMManager.GetPlayer(ReceiverPlayerUUID);
                if(senderGO && receiverGO)
                {
                    PlayerInfo psendInfo = senderGO.GetComponent<PlayerInfo>();
                    PlayerInfo preceInfo = receiverGO.GetComponent<PlayerInfo>();

                    if(psendInfo && preceInfo)
                    {
                        psendInfo.ThePlayerData.playerKills += 1;
                        preceInfo.ThePlayerData.playerDeaths += 1;
                    }

                    GlobalPlayerManager.NetworkMessagePlayerUpdate msg = new GlobalPlayerManager.NetworkMessagePlayerUpdate();
                    msg.PlayerNetID = psendInfo.GetComponent<NetworkIdentity>().netId.Value;
                    msg.deaths = psendInfo.ThePlayerData.playerDeaths;
                    msg.kills  = psendInfo.ThePlayerData.playerKills;
                    msg.playerTag = psendInfo.ThePlayerData.playerTag;
                    msg.playerUUID = psendInfo.ThePlayerData.playerUUID;
                    NetworkServer.SendToAll(4001, msg);

                    GlobalPlayerManager.NetworkMessagePlayerUpdate msg1 = new GlobalPlayerManager.NetworkMessagePlayerUpdate();
                    msg1.PlayerNetID = preceInfo.GetComponent<NetworkIdentity>().netId.Value;
                    msg1.deaths = preceInfo.ThePlayerData.playerDeaths;
                    msg1.kills  = preceInfo.ThePlayerData.playerKills;
                    msg.playerTag = preceInfo.ThePlayerData.playerTag;
                    msg.playerUUID = preceInfo.ThePlayerData.playerUUID;
                    NetworkServer.SendToAll(4001, msg1);
                }
            }
        }

        [ClientRpc]
        void RpcRespawn()
        {
            if(isLocalPlayer)
            {
                // fire the event player died
                EventMessenger.Instance.Raise(new EventPlayerDied(ThePlayerInfo.ThePlayerData.playerUUID));
            }
        }

        public void RespawnPlayerAtRandom()
        {
            // put player to one random spawn point
            TheTransform.position = GetRandomSpawnPoint();
        }

        private Vector3 GetRandomSpawnPoint()
        {
            // Set the spawn point to origin as a default value
            Vector3 result = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                result = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // return the position to the chosen spawn point
            return result;
        }
	}
}