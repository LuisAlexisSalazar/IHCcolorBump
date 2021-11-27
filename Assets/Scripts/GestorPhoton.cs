using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GestorPhoton : MonoBehaviourPunCallbacks
{
    private bool existBall = false;

    void Start()
    {
        //Conexión al servidor
        PhotonNetwork.ConnectUsingSettings();
    }

    //Conectar al servidor
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    //Unirse al lobby
    public override void OnJoinedLobby()
    {
        //Unirse a un room ya creado o creara el room si no existe
        PhotonNetwork.JoinOrCreateRoom("Cuarto", new RoomOptions {MaxPlayers = 2},
            TypedLobby.Default);
    }

    //Que va ocurrir cuando nos unamos al cuarto
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            existBall = true;

        if (existBall)
        {
            Debug.Log("Jugador 2 debe controlar balon 1");
            // PhotonNetwork.Instantiate("MovSecondPlayer", new Vector3(-0.2f, -0.15f, 2.41f),
            //     Quaternion.identity);
            // MovSecondPlayer
            // PhotonNetwork.Instantiate("controlPlayerOne", new Vector3(-0.2f, -0.15f, 2.41f),
            //     Quaternion.identity);
            GameObject Control2 = PhotonNetwork.Instantiate("control",
                new Vector3(-0.2f, -0.15f, 2.41f),
                Quaternion.identity);

            Control ControlPlayer2 = Control2.GetComponent<Control>();
            ControlPlayer2.UserID = 2;
            ControlPlayer2.connectionpPort = 50002;
        }

        else
        {
            Debug.Log("Jugador 1 Creación del Balon");
            // Objeto de la carpeta resources , dodne se instanciara
            PhotonNetwork.Instantiate("SplitMetalBall", new Vector3(-0.2f, -0.15f, 2.41f),
                Quaternion.identity);

            PhotonNetwork.Instantiate("controlAudio", new Vector3(-0.2f, -0.15f, 2.41f),
                Quaternion.identity);

            GameObject Control1 = PhotonNetwork.Instantiate("control",
                new Vector3(-0.2f, -0.15f, 2.41f),
                Quaternion.identity);

            Control ControlPlayer1 = Control1.GetComponent<Control>();
            ControlPlayer1.UserID = 1;
            ControlPlayer1.connectionpPort = 50001;
        }
    }
}