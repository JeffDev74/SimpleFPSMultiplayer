using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(CanvasGroup))]
	public class UIRespawnPanel : MonoBehaviour, IUIPanel
	{
        private string _panelName = "RespawnPanel";
        public string PanelName
        {
            get { return _panelName; }
            set { _panelName = value; }
        }

        private CanvasGroup _theCanvasGroup;
        private CanvasGroup TheCanvasGroup
        {
            get
            {
                if(_theCanvasGroup == null)
                {
                    _theCanvasGroup = GetComponent<CanvasGroup>();
                }
                return _theCanvasGroup;
            }
        }

        public UIManager TheUIManager;

        private void Awake()
        {
            TheUIManager.RegisterPanel(gameObject);
        }

        public void TogglePanel(bool state)
        {
            if (state)
            {
                ShowPanel();
            }
            else
            {
                HidePanel();
            }
        }

        public void ShowPanel()
        {
            TheCanvasGroup.alpha = 1;
            TheCanvasGroup.interactable = true;
            TheCanvasGroup.blocksRaycasts = true;
        }

        public void HidePanel()
        {
            TheCanvasGroup.alpha = 0;
            TheCanvasGroup.interactable = false;
            TheCanvasGroup.blocksRaycasts = false;
        }
	}
}