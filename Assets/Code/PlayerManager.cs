using UnityEngine;

namespace FPS
{
	public class PlayerManager : MonoBehaviour
	{
        public Camera _thePlayerCamera;
        public Camera ThePlayerCamera
        {
            get { return _thePlayerCamera; }
        }
    }
}