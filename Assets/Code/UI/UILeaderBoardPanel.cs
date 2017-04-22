using System;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UILeaderBoardPanel : MonoBehaviour, IUIPanel
    {
        private Transform _theTransform;
        public Transform TheTransform
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

        private string _panelName = Helper.PanelNames.LeaderBoardsPanel;
        public string PanelName
        {
            get { return _panelName; }
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

        public LeaderBoard TheLeaderBoardComponent;

        public void TogglePanel(bool state)
        {
            TheLeaderBoardComponent.PanelToggle(state);
            if (state)
            {
                ShowPanel();
            }
            else
            {
                HidePanel();
            }
        }

        private void ShowPanel()
        {
            TheCanvasGroup.alpha = 1;
            TheCanvasGroup.interactable = true;
            TheCanvasGroup.blocksRaycasts = true;
        }

        private void HidePanel()
        {
            TheCanvasGroup.alpha = 0;
            TheCanvasGroup.interactable = false;
            TheCanvasGroup.blocksRaycasts = false;
        }
    }
}