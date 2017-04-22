using UnityEngine;

namespace FPS
{
	public interface IUIPanel
	{
        Transform TheTransform { get; }
        string PanelName { get; }
        void TogglePanel(bool state);
	}
}