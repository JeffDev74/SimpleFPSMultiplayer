using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class PlayerManager : NetworkBehaviour
	{
        public Camera _thePlayerCamera;
        public Camera ThePlayerCamera
        {
            get { return _thePlayerCamera; }
        }
    }
}