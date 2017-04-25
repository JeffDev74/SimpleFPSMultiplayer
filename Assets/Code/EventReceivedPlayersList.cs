using System.Collections.Generic;
using FPS.EventSystem;
using UnityEngine;

namespace FPS
{
	public class EventReceivedPlayersList : GameEvent
	{
	    private List<GameObject> _players;

        public List<GameObject> Players
	    {
	        get { return _players; }
            private  set { _players = value; }
	    }

	    public EventReceivedPlayersList(List<GameObject> players)
	    {
	        this.Players = players;
	    }
    }
}