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
            return null;
        }

        public void RegisterPanel(GameObject panel)
        {
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

        private void Start()
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
                TogglePanel("RespawnPanel", state);
            }
        }
    }
}