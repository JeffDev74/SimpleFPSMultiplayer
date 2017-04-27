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

        private CharacterController _theCharController;
        private CharacterController TheCharController
        {
            get
            {
                if (_theCharController == null)
                {
                    _theCharController = GetComponent<CharacterController>();
                }
                return _theCharController;
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

        public float speed = 3.0F;
        public float rotateSpeed = 3.0F;
        void Update()
        {
            if (CanMove == false) return;
                
            if (isLocalPlayer == false)
            {
                return;
            }

            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
            //Quaternion.LookDirection(forward, Vector3.up)
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            float curSpeed = speed * Input.GetAxis("Vertical");
            TheCharController.SimpleMove(forward * curSpeed);

            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
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