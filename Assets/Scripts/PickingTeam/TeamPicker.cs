using UnityEngine;
using MLAPI;
using MLAPI.Connection;

public class TeamPicker : MonoBehaviour
{
    #region Variables
    #endregion
    #region Functions
    public void SelectTeam(int teamIndex)
    {
        var localClientId = NetworkManager.Singleton.LocalClientId;
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            if (networkClient.PlayerObject.TryGetComponent<TeamPlayer>(out var teamPlayer))
            {
                teamPlayer.SetTeamServerRpc((byte)teamIndex);


            }

        }
    }
    #endregion
}
