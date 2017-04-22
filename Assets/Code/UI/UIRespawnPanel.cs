﻿using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(CanvasGroup))]
	public class UIRespawnPanel : MonoBehaviour, IUIPanel
	{
        private Transform _theTransform;
        public Transform TheTransform
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

        private string _panelName = Helper.PanelNames.RespawnPanel;
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