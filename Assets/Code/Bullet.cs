using UnityEngine;

namespace FPS
{
	public class Bullet : MonoBehaviour
	{
        public string senderPlayerUUID;

        private void OnCollisionEnter(Collision collision)
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<Health>();

            if (health != null)
            {
                string receiverPlayerUUID = System.Guid.Empty.ToString();

                PlayerInfo thePlayerInfo = hit.GetComponent<PlayerInfo>();
                if(thePlayerInfo != null)
                {
                    receiverPlayerUUID = thePlayerInfo.ThePlayerData.playerUUID;
                }

                health.TakeDamage(10, senderPlayerUUID, receiverPlayerUUID);
            }

            Destroy(gameObject);
        }
    }
}