using UnityEngine;

namespace FPS
{
	public class Billboard : MonoBehaviour
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

        private Camera _mainCamera;
        private Camera MainCamera
        {
            get
            {
                if(_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }
                return _mainCamera;
            }
        }

        private void Update()
        {
            TheTransform.LookAt(MainCamera.transform);
        }
    }
}