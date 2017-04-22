using UnityEngine;

namespace FPS
{
	public interface IUIPanel
	{
        string PanelName { get; set; }
        void TogglePanel(bool state);
	}
}