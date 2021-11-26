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

    public void Create_Room()
    {
        numero = 1;
        PhotonNetwork.JoinOrCreateRoom("Room: " + numero, new RoomOptions() { MaxPlayers = 2}, TypedLobby.Default);
        Debug.Log("Sala Creada");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Erro ! Sala no creada");
        Create_Room();
       //base.OnCreateRoomFailed(returnCode, "Error no se pudo crear la sala");
    }

}
