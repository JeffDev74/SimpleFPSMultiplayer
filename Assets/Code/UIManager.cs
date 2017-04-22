using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
	public class UIManager : MonoBehaviour
	{
        List<GameObject> _panels;
        List<GameObject> Panels
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
            // got look the list
            //UIPanel panel = null;
            for (int i = 0; i < Panels.Count; i++)
            {
                //panel = Panels[i].GetComponent<UIPanel>();
                //if(panel  != null)
                //{
                //    // check the name
                //    panel.TogglePanel(state);
                //}
            }
        }
	}
}