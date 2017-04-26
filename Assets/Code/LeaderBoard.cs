using FPS.EventSystem;
using UnityEngine;

namespace FPS
{
	public class LeaderBoard : MonoBehaviour
	{
        public GameObject TableRowPrefab;

        public Transform RowsContainer;

	    void OnEnable()
	    {
	        EventMessenger.Instance.AddListner<EventReceivedPlayersList>(OnReceivedPlayersList);
	    }

	    void OnDisable()
	    {
	        EventMessenger.Instance.RemoveListner<EventReceivedPlayersList>(OnReceivedPlayersList);
        }

        private void OnReceivedPlayersList(EventReceivedPlayersList e)
        {
            ClearAllRows();
            foreach (GameObject go in e.Players)
            {
                PlayerInfo pInfo = go.GetComponent<PlayerInfo>();
                if(pInfo != null)
                {
                    GameObject instantiatedrow = Instantiate(TableRowPrefab, Vector3.zero, Quaternion.identity);
                    UIRowPlayerInfo rowInfo = instantiatedrow.GetComponent<UIRowPlayerInfo>();
                    if(rowInfo != null)
                    {
                        rowInfo.UpdateRowText(pInfo.ThePlayerData.playerTag, pInfo.ThePlayerData.playerKills, pInfo.ThePlayerData.playerDeaths);
                        instantiatedrow.transform.SetParent(RowsContainer);
                    }
                }
            }
        }

        public void PanelToggle(bool state)
        {
            if(state)
            {
                GlobalPlayerManager.Instance.RequestPlayersList();
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