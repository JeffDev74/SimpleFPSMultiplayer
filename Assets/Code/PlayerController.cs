using FPS.EventSystem;
using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
    public class PlayerController : NetworkBehaviour
    {
        private Transform _theTransform;
        private Transform TheTransform
        {
            get
            {
                if (_theTransform == null)
                {
                    _theTransform = transform;
                }
                return _theTransform;
            }
        }

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

        public GameObject BulletPrefab;
        public Transform bulletspawn;

        private Collider _thePlayerCollider;
        public Collider ThePlayerCollider
        {
            get
            {
                if (_thePlayerCollider == null)
                {
                    _thePlayerCollider = GetComponent<Collider>();
                }
                return _thePlayerCollider;
            }
        }

        public bool CanMove = true; 

        private void OnEnable()
        {
            EventMessenger.Instance.AddListner<EventPlayerDied>(OnPlayeDied);
            EventMessenger.Instance.AddListner<EventPlayerRespawn>(OnPlayeRespawn);
            
        }

        private void OnDisable()
        {
            EventMessenger.Instance.RemoveListner<EventPlayerDied>(OnPlayeDied);
            EventMessenger.Instance.RemoveListner<EventPlayerRespawn>(OnPlayeRespawn);
        }

        private void OnPlayeRespawn(EventPlayerRespawn e)
        {
            if (isLocalPlayer)
            {
                CmdTogglePlayer(true);
            }
        }

        private void OnPlayeDied(EventPlayerDied e)
        {
            if (isLocalPlayer)
            {
                CmdTogglePlayer(false);
            }
        }

        [Command]
        void CmdTogglePlayer(bool state)
        {
            TogglePlayer(state);
            TogglePlayerMeshRenders(state);
            ThePlayerInfo.PlayerHud.TogglePanel(state);
            RpcTogglePlayer(state);
        }

        [ClientRpc]
        void RpcTogglePlayer(bool state)
        {
            TogglePlayer(state);
            TogglePlayerMeshRenders(state);
            ThePlayerInfo.PlayerHud.TogglePanel(state);
        }


        private void Update()
        {
            if(CanMove == false) return;
            
            if (isLocalPlayer == false)
            {
                return;
            }

            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            TheTransform.Rotate(0, x, 0);
            TheTransform.Translate(0, 0, z);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            //if(Input.GetKeyDown(KeyCode.Space))
            {
                CmdFire();
            }

        }

        public void TogglePlayer(bool state)
        {
            CanMove = state;
            ThePlayerCollider.enabled = state;
        }

        public void TogglePlayerMeshRenders(bool state)
        {
            MeshRenderer playerRender = GetComponent<MeshRenderer>();
            if (playerRender != null)
            {
                playerRender.enabled = state;
            }

            MeshRenderer[] renderes = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderes.Length; i++)
            {
                renderes[i].enabled = state;
            }
        }

        public override void OnStartLocalPlayer()
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        // This [Command] code is called on the Client ...
        // ... but it is run on the Server!
        [Command]
        void CmdFire()
        {
            // Create the bullet from the bullet prefab
            var instantiatedBullet = Instantiate(BulletPrefab, bulletspawn.position, bulletspawn.rotation);

            Bullet bulletComponent = instantiatedBullet.GetComponent<Bullet>();
            if(bulletComponent != null)
            {
                bulletComponent.senderPlayerUUID = ThePlayerInfo.ThePlayerData.playerUUID;
            }

            // Add velocity to the bullet
            instantiatedBullet.GetComponent<Rigidbody>().velocity = instantiatedBullet.transform.forward * 6;

            // Spawn the bullet to clients
            NetworkServer.Spawn(instantiatedBullet);

            // Destroy the bullet after 2 seconds
            Destroy(instantiatedBullet, 2.0f);
        }
    }
}