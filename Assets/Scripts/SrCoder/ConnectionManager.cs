using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Connection;
using TMPro;
using MLAPI.Transports.UNET;

public class ConnectionManager : NetworkBehaviour
{
    
    #region Variables
    [SerializeField] GameObject connectionUI;
    [SerializeField] GameObject LeaveButton;
    [SerializeField] GameObject mainCamera;
    public string iPAddress="127.0.0.1";

    UNetTransport transport;
    #endregion
    #region Monobehaviour callbacks
    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        NetworkManager.Singleton.OnServerStarted += serverStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += clientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += clientDisconected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null)
            return;
        NetworkManager.Singleton.OnServerStarted -= serverStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= clientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= clientDisconected;
    }
    #endregion
    #region Functions
    public void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(GetRandomSpawnPos(), Quaternion.identity);
        connectionUI.SetActive(false);
        mainCamera.SetActive(false);
    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            NetworkManager.Singleton.StopHost();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient();
        }
        //LeaveButton.SetActive(false);
        //connectionUI.SetActive(true);
        //mainCamera.SetActive(true);
        //GameManager.instance.CanvasHpUiElements.backGoundObj.SetActive(false);
        switchUi(false);
    }

    public void Join()
    {
        NetworkManager.Singleton.TryGetComponent(out transport);
        transport.ConnectAddress = iPAddress;
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("CustomPassword");
        NetworkManager.Singleton.StartClient();
    }
    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callBack)
    {

        bool aprrove = System.Text.Encoding.ASCII.GetString(connectionData) == "CustomPassword";
        callBack(true, null, aprrove, GetRandomSpawnPos(), Quaternion.identity);
    }

    private void clientDisconected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            //connectionUI.SetActive(true);
            //mainCamera.SetActive(true);
            //LeaveButton.SetActive(false);
            switchUi(false);
        }
    }

    private void clientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            //connectionUI.SetActive(false);
            //mainCamera.SetActive(false);
            //LeaveButton.SetActive(true);
            switchUi(true);
        }
    }

    private void serverStarted()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            clientConnected(NetworkManager.Singleton.LocalClientId);
        }
    }

    Vector3 GetRandomSpawnPos()
    {
        return new Vector3(getRandomNumInRange(), 2, getRandomNumInRange());
    }
    float getRandomNumInRange()
    {
        return Random.Range(-5.0f, 5.0f);
    }
    public void IpAdderssChanged(string newAddress)
    {
        iPAddress = newAddress;
    }
    public void switchUi(bool isPlayerSpawning)
    {
        LeaveButton.SetActive(isPlayerSpawning);
        connectionUI.SetActive(!isPlayerSpawning);
        mainCamera.SetActive(!isPlayerSpawning);
        GameManager.instance.CanvasHpUiElements.backGoundObj.SetActive(isPlayerSpawning);
    }
    #endregion
}
