
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GestorPhoton : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //Conexión al servidor
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
    }

    //Conectar al servidor
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    //Unirse al lobby
    public override  void OnJoinedLobby()
    {
        //Unirse a un room ya creado o creara el room si no existe
        PhotonNetwork.JoinOrCreateRoom("Cuarto", new RoomOptions {MaxPlayers = 2},
            TypedLobby.Default);
    }

    //Que va ocurrir cuando nos unamos al cuarto
    public override void OnJoinedRoom()
    {
        //Objeto de la carpeta resources , dodne se instanciara
        PhotonNetwork.Instantiate("SplitMetalBall", new Vector3(-0.2f, -0.15f, 2.41f),
            Quaternion.identity);
        
        
    }
}