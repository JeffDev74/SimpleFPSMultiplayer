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

        [SyncVar(hook = "OnChangeHealth")]
        public int currentHealth = maxHealth;

        void OnChangeHealth(int health)
        {
            currentHealth = health;
            healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        }

        public RectTransform healthBar;

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if(currentHealth <= 0)
            {
                currentHealth = maxHealth;

                // called on the server but invoked on the clients
                RpcRespawn();
            }
        }

        [ClientRpc]
        void RpcRespawn()
        {
            if(isLocalPlayer)
            {
                TheTransform.position = Vector3.zero;
            }
        }
	}
}