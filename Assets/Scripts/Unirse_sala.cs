using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Unirse_sala : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Se unio a la sala exitosamente");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Error !  No se pudo usar");
        Join();
    }

}
