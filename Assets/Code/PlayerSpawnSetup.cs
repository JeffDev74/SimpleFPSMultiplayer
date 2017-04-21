using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class PlayerSpawnSetup : NetworkBehaviour
	{
        Camera _overviewCamera;
        Camera OverviewCamera
        {
            get
            {
                if(_overviewCamera == null)
                {
                    GameObject go = GameObject.FindGameObjectWithTag(Helper.GameTags.overviewCamera);
                    if(go != null)
                    {
                        _overviewCamera = go.GetComponent<Camera>();
                    }
                    else
                    {
                        Debug.LogError("Cannot find the game object with tag [" + Helper.GameTags.overviewCamera + "]", gameObject);
                    }
                }
                return _overviewCamera;
            }
        }
        PlayerCamera _thePlayerCam;
        PlayerCamera ThePlayerCam
        {
            get
            {
                if(_thePlayerCam == null)
                {
                    _thePlayerCam = GetComponent<PlayerCamera>();
                }
                return _thePlayerCam;
            }
        }
        public override void OnStartLocalPlayer()
        {
            OverviewCamera.gameObject.SetActive(false);
        }
    }
}