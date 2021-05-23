using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using System;

public class PlayerHealth : NetworkBehaviour
{
    #region Variables
    NetworkVariableInt health = new NetworkVariableInt(100);
    [SerializeField] int acutalHealth = 100;
    [SerializeField] HpUIElements HoveringPlayerHpUIElements;
    #endregion
    #region Monobehaviour Callbacks
    private void OnEnable()
    {
        acutalHealth = health.Value;
        health.OnValueChanged += onHealthChange;
        HoveringPlayerHpUIElements.backGoundObj.SetActive(!IsLocalPlayer);

    }
    private void OnDisable()
    {
        health.OnValueChanged -= onHealthChange;
    }
    #endregion
    #region Functions


    private void onHealthChange(int previousValue, int newValue)
    {
        acutalHealth = newValue;
        if (!IsLocalPlayer)
        {
            HoveringPlayerHpUIElements .healthText.text = acutalHealth.ToString();
            HoveringPlayerHpUIElements.foreImage.fillAmount = (float)acutalHealth / 100.0f;
        }
        HealthBarChangeServerRpc();
    }

    public void takeDamage(int damage)
    {
        health.Value -= damage;
    }
    [ServerRpc]
    public void HealthBarChangeServerRpc()
    {
        HealthBarChangeClientRpc();
    }
    [ClientRpc]
    public void HealthBarChangeClientRpc()
    {
        if (IsLocalPlayer)
        {
            GameManager.instance.CanvasHpUiElements.healthText.text = acutalHealth.ToString();
            GameManager.instance.CanvasHpUiElements.foreImage.fillAmount = (float)acutalHealth / 100.0f;
        }
    }
    #endregion
}
