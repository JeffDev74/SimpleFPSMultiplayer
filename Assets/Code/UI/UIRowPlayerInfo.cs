using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
	public class UIRowPlayerInfo : MonoBehaviour
	{
        public Text PlayerTag;
        public Text PlayerKills;
        public Text PlayerDeaths;

        public void UpdateRowText(string tag, int kills, int deaths)
        {
            PlayerTag.text = tag;
            PlayerKills.text = kills.ToString();
            PlayerDeaths.text = deaths.ToString();
        }
	}
}