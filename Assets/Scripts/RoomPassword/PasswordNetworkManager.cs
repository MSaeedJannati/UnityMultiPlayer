using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using TMPro;
using System.Text;
using System;

namespace NetworkRoomPassword
{
    public class PasswordNetworkManager : MonoBehaviour
    {
        #region Variabels
        [SerializeField] private TMP_InputField passwrodInputField;
        [SerializeField] private GameObject passwrodEntryUI;
        [SerializeField] private GameObject leaveButton;
        [SerializeField] private GameObject wrongPasswordLabel;
        WaitForSeconds delay = new WaitForSeconds(2.0f);
        #endregion
        #region Monobehaviour callbacks
        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += HandleSeverStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconected;
        }

       

        private void OnDestroy()
        {
            if (NetworkManager.Singleton == null)
                return;
            NetworkManager.Singleton.OnServerStarted -= HandleSeverStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconected;
        }
        #endregion
        #region Functions
        public void Host()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost(new Vector3(-2f,1,0),Quaternion.Euler(0f,135f,0f));
        }



        public void Client()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(passwrodInputField.text);
            NetworkManager.Singleton.StartClient();
        }
        public void Leave()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.StopHost();
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StopClient();
            }
            passwrodEntryUI.SetActive(true);
            leaveButton.SetActive(false);
        }
        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            string password = Encoding.ASCII.GetString(connectionData);

            bool aproveConnection = password == passwrodInputField.text;

          
            Vector3 spawnPos=Vector3.zero;
            Quaternion spanwRot=Quaternion.identity;
            switch (NetworkManager.Singleton.ConnectedClients.Count)
            {
                case  1:
                    spawnPos = new Vector3(0f, 1f, 0f); ;
                    spanwRot = Quaternion.Euler(0f, 180f, 0f);
                    break;
                case 2:
                    spawnPos = new Vector3(2f,1f,0f);
                    spanwRot = Quaternion.Euler(0f, 225f, 0f);
                    break;
            }
            callback(true, null, aproveConnection, spawnPos, spanwRot);
        }
        private void HandleSeverStarted()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                HandleClientConnected(NetworkManager.Singleton.LocalClientId);
            }
        }
        private void HandleClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                passwrodEntryUI.SetActive(false);
                leaveButton.SetActive(true);
            }
        }
        private void HandleClientDisconected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                passwrodEntryUI.SetActive(true);
                leaveButton.SetActive(false);
            }
        }




        #endregion
        #region Coroutines
        public IEnumerator showWrongPassCoroutine()
        {
            wrongPasswordLabel.SetActive(true);
            yield return delay;
            wrongPasswordLabel.SetActive(false);
        }
        #endregion
    }

}