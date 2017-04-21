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

        public GameObject BulletPrefab;
        public Transform bulletspawn;

        private void Update()
        {
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

        public override void OnStartLocalPlayer()
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        // This [Command] code is called on the Client …
        // … but it is run on the Server!
        [Command]
        void CmdFire()
        {
            // Create the bullet from the bullet prefab
            var bullet = Instantiate(BulletPrefab, bulletspawn.position, bulletspawn.rotation);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

            // Spawn the bullet to clients
            NetworkServer.Spawn(bullet);

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }
    }
}