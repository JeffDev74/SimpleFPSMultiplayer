using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
	public class MainMenu : MonoBehaviour
	{
        private GameNetworkManager _theGameNetworkManager;
        private GameNetworkManager TheGameNetworkManager
        {
            get
            {
                if(_theGameNetworkManager == null)
                {
                    _theGameNetworkManager = FindObjectOfType<GameNetworkManager>();
                }
                return _theGameNetworkManager;
            }
        }

        [SerializeField]
        private Button _playBtn;
        [SerializeField]
        private Button _hostBtn;

        [SerializeField]
        private InputField _playerTag;

        private void OnEnable()
        {
            _playBtn.onClick.AddListener(OnPlayBtnClick);
            _hostBtn.onClick.AddListener(OnHostBtnClick);
        }

        private void OnDisable()
        {
            _playBtn.onClick.RemoveAllListeners();
            _hostBtn.onClick.RemoveAllListeners();
        }

        #region Button Click Handlers

        private void OnHostBtnClick()
        {
            TheGameNetworkManager.StartHost();
        }

        private void OnPlayBtnClick()
        {
            string playerTag = _playerTag.text;

            if(string.IsNullOrEmpty(playerTag) == false)
            {
                TheGameNetworkManager.GameStartClient(playerTag);
            }
        }

        #endregion Button Click Handlers
    }
}