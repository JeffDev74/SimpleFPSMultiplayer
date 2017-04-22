using FPS.EventSystem;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
	public class UIManager : MonoBehaviour
	{
        bool state = false;

        private List<GameObject> _panels;
        private List<GameObject> Panels
        {
            get
            {
                if(_panels == null)
                {
                    _panels = new List<GameObject>();
                }
                return _panels;
            }
        }

        public GameObject GetPanelByName(string panelName)
        {
            IUIPanel panel = null;
            for (int i = 0; i < Panels.Count; i++)
            {
                panel = Panels[i].GetComponent<IUIPanel>();
                if (panel != null)
                {
                    if (panel.PanelName == panelName)
                    {
                        return Panels[i];
                    }
                }
            }

            return null;
        }

        public void RegisterPanel(GameObject panel)
        {
            // NOTE: add check and only add panel if is not in the list
            Panels.Add(panel);
        }

        public void TogglePanel(string panelName, bool state)
        {
            IUIPanel panel = null;
            for (int i = 0; i < Panels.Count; i++)
            {
                panel = Panels[i].GetComponent<IUIPanel>();
                if (panel != null)
                {
                    if(panel.PanelName == panelName)
                    {
                        panel.TogglePanel(state);
                    }
                }
            }
        }

        private void OnEnable()
        {
            EventMessenger.Instance.AddListner<EventPlayerDied>(OnPlayeDied);
        }

        private void OnDisable()
        {
            EventMessenger.Instance.RemoveListner<EventPlayerDied>(OnPlayeDied);
        }

        private void OnPlayeDied(EventPlayerDied e)
        {
            DisableAllPanels();
            TogglePanel(Helper.PanelNames.RespawnPanel, true);
        }

        private void Start()
        {
            // If this script is moved to other object 
            // we need to refactore this to register panels
            IUIPanel[] _panelsList = GetComponentsInChildren<IUIPanel>();
            for (int i = 0; i < _panelsList.Length; i++)
            {
                RegisterPanel(_panelsList[i].TheTransform.gameObject);
            }

            DisableAllPanels();
        }

        private void DisableAllPanels()
        {
            IUIPanel panel = null;
            for (int i = 0; i < Panels.Count; i++)
            {
                panel = Panels[i].GetComponent<IUIPanel>();
                if (panel != null)
                {
                    panel.TogglePanel(false);
                }
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                state = !state;
                TogglePanel(Helper.PanelNames.LeaderBoardsPanel, state);
            }
        }
    }
}