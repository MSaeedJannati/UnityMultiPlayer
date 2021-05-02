using UnityEngine;
using MLAPI;
using MLAPI.Messaging;


public class ParticleSpawner : NetworkBehaviour
{
    #region variables
    [SerializeField] private GameObject particleObject;
    #endregion
    #region Monobehaviour callbacks
    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spawnParticleServerRpc();
                Instantiate(particleObject, transform.position, transform.rotation);
            }
        }
    }
    #endregion
    #region Functions
    [ServerRpc]
    void spawnParticleServerRpc()
    {
        spawnParticleClientRpc();
    }
    [ClientRpc]
    private void spawnParticleClientRpc()
    {
        if (!IsOwner)
            Instantiate(particleObject, transform.position, transform.rotation);
    }
    #endregion
}
