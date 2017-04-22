using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(CanvasGroup))]
	public class UIRespawnPanel : MonoBehaviour
	{
        public string PanelName = "Unknown";

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

        public void TogglePanel(bool state)
        {
            if(state)
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