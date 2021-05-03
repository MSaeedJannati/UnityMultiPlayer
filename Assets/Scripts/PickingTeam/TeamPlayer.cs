using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class TeamPlayer : NetworkBehaviour
{
    #region Variables
    [SerializeField] private Renderer teamColourRenderer;
    [SerializeField] private Color[] teamColours;

    private NetworkVariable<byte> teamIndex = new NetworkVariable<byte>();
    #endregion
    #region Monobehaviour callbacks
    private void OnEnable()
    {
        teamIndex.OnValueChanged += onTeamChange;
    }
    private void OnDisable()
    {
        teamIndex.OnValueChanged -= onTeamChange;
    }
    #endregion
    #region Functions
    [ServerRpc]
    public void SetTeamServerRpc(byte newTeamIndex)
    {
        if (newTeamIndex > 3)
            return;

        teamIndex.Value = newTeamIndex;
    }
    private void onTeamChange(byte oldTeamIndex, byte newTeamIndex)
    {
        if (!IsClient)
            return;
        teamColourRenderer.material.SetColor("_BaseColor", teamColours[newTeamIndex]);
    }
    #endregion
}
