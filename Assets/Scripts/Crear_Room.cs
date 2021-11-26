using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Crear_Room : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public int numero;

    void Start()
    {
        
    }

    public void crear_sala()
    {
        numero = Random.Range(1,100);
        Debug.Log("Intentando crear sala");
        //PhotonNetwork.DestroyAll();
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinOrCreateRoom("Room: " + numero, new RoomOptions() { MaxPlayers = 2}, TypedLobby.Default);
        Debug.Log("Sala Creada");
        //Debug.Log(PhotonNetwork.CurrentRoom.numrt)
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Erro ! Sala no creada");
        crear_sala();
       //base.OnCreateRoomFailed(returnCode, "Error no se pudo crear la sala");
    }

}
