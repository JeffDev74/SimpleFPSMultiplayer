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

        private NetworkStartPosition[] spawnPoints;

        public RectTransform healthBar;

        [SyncVar(hook = "OnChangeHealth")]
        public int currentHealth = maxHealth;

        void OnChangeHealth(int health)
        {
            currentHealth = health;
            healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
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

                }

            }
        }

        [ClientRpc]
        void RpcRespawn()
        {
            if(isLocalPlayer)
            {
                TheTransform.position = GetRandomSpawnPoint();
            }
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