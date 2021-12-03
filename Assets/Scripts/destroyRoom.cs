using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class destroyRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void destroy()
    {
        PhotonNetwork.DestroyAll();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
    }
}
