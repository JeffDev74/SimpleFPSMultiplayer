using UnityEngine;

namespace FPS
{
    public class PlayerController : MonoBehaviour
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

        private void Update()
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            TheTransform.Rotate(0, x, 0);
            TheTransform.Translate(0, 0, z);
        }
    }
}