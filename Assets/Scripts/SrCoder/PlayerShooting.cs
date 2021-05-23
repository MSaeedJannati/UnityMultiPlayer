using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class PlayerShooting : NetworkBehaviour
{
    #region Variables
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] Transform gunBarrel;
    #endregion
    #region Monobehaviour callbacks
    #endregion
    #region Functions
    private void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                shootServerRpc();
            }
        }

    }
    [ServerRpc]
    void shootServerRpc()
    {
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out var hit, 100f))
        {
            hit.transform.TryGetComponent<PlayerHealth>(out var enemyHealth);
            if (enemyHealth != null)
            {
                enemyHealth.takeDamage(10);
            }
        }
        shootClientRpc();
    }
    [ClientRpc]
    void shootClientRpc()
    {
        var bullet = Instantiate(bulletTrail, gunBarrel.position, Quaternion.identity);
        bullet.AddPosition(gunBarrel.position);
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out var hit,100f))
        {
            bullet.transform.position = hit.point;
        }
        else
        {
            bullet.transform.position = gunBarrel.position+gunBarrel.forward*100f;
        }
    }
    #endregion
}
