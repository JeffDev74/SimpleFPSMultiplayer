using FPS.EventSystem;

namespace FPS
{
	public class EventPlayerDied : GameEvent
	{
        private string _playerUUID;
        public string PlayerUUID
        {
            get { return _playerUUID; }
            private set { _playerUUID = value; }
        }

        public EventPlayerDied(string _playerUUID)
        {
            PlayerUUID = _playerUUID;
        }
    }
}