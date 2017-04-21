using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
	public class GameNetworkManager : NetworkManager
	{
        //public GameObject ourPlayerPrefab;

        //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        //{
        //    GameObject instantiatedPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //    NetworkServer.AddPlayerForConnection(conn, instantiatedPlayer, playerControllerId);
        //}
    }
}