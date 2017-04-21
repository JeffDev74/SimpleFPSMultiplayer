using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class PlayerCamera : NetworkBehaviour
	{
        [SerializeField]
        private PlayerManager _thePlayerManager;
        private PlayerManager ThePlayerManager
        {
            get
            {
                return _thePlayerManager;
            }
            set { _thePlayerManager = value; }
        }

        private Transform _theTransform;
        private Transform TheTransform
        {
            get
            {
                if(_theTransform == null)
                {
                    _theTransform = GetComponent<Transform>();
                }
                return _theTransform;
            }
        }

        public override void PreStartClient()
        {
            ThePlayerManager = FindObjectOfType<PlayerManager>();

            ThePlayerManager.ThePlayerCamera.transform.position = new Vector3(0, 0.5f, 0);
            ThePlayerManager.ThePlayerCamera.transform.SetParent(TheTransform);
        }
    }
}