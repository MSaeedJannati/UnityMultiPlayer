using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        #region Variables

        public NetworkVariableVector3 Position = new NetworkVariableVector3(
            new NetworkVariableSettings
            {
                WritePermission = NetworkVariablePermission.ServerOnly,
                ReadPermission = NetworkVariablePermission.Everyone
            }
            );
        #endregion
        #region Monobehaviour CallBacks
        private void Update()
        {
            transform.position = Position.Value;
        }
        #endregion
        #region Functions
        public override void NetworkStart()
        {
            Move();
        }
        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }
        [ServerRpc]
        void SubmitPositionRequestServerRpc()
        {
            Position.Value = GetRandomPositionOnPlane();
        }
        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }
        #endregion

    }
}