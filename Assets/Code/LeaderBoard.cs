using UnityEngine;

namespace FPS
{
	public class LeaderBoard : MonoBehaviour
	{
        GameNetworkManager _GNManager;
        GameNetworkManager GNManager
        {
            get
            {
                if(_GNManager == null)
                {
                    _GNManager = FindObjectOfType<GameNetworkManager>();
                }
                return _GNManager;
            }
        }

        public GameObject TableRowPrefab;

        public Transform RowsContainer;

        public void PanelToggle(bool state)
        {
            if(state)
            {
                GlobalPlayerManager.Instance.RequestPlayersList();
                
                //ClearAllRows();
                //foreach (GameObject go in GNManager.ConnectedPlayers)
                //{
                //    PlayerInfo pInfo = go.GetComponent<PlayerInfo>();
                //    if(pInfo != null)
                //    {
                //        GameObject instantiatedrow = Instantiate(TableRowPrefab, Vector3.zero, Quaternion.identity);
                //        UIRowPlayerInfo rowInfo = instantiatedrow.GetComponent<UIRowPlayerInfo>();
                //        if(rowInfo != null)
                //        {
                //            rowInfo.UpdateRowText(pInfo.ThePlayerData.playerTag, pInfo.ThePlayerData.playerKills, pInfo.ThePlayerData.playerDeaths);
                //            instantiatedrow.transform.SetParent(RowsContainer);
                //        }
                //    }
                //}
            }
        }

        private void ClearAllRows()
        {
            foreach (Transform t in RowsContainer.transform)
            {
                Destroy(t.gameObject);
            }
        }
	}
}