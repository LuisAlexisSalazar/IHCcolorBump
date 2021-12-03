using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class GestorPhoton : MonoBehaviourPunCallbacks
{
    private bool existBall = false;
    TextMeshProUGUI scoreText;
    private int player_count;

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
        int numero;
        numero = UnityEngine.Random.Range(1, 100);
        Debug.Log("Intentando crear sala");

        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LeaveLobby();
        //PhotonNetwork.JoinOrCreateRoom("Room: " + numero, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
        PhotonNetwork.JoinOrCreateRoom("Cuarto", new RoomOptions {MaxPlayers = 2},
            TypedLobby.Default);
        Debug.Log("Sala Creada");
        /*PhotonNetwork.JoinOrCreateRoom("Cuarto", new RoomOptions {MaxPlayers = 2},
            TypedLobby.Default);*/
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            existBall = true;

        if(existBall)
        {
            scoreText = GameObject.FindGameObjectWithTag("text1").GetComponent<TextMeshProUGUI>();

        }
    }

    void Update()
    {
        try 
        {
            player_count = PhotonNetwork.CurrentRoom.PlayerCount;
        }
        catch
        {
            Debug.Log("Player count");
        }
        //player_count = PhotonNetwork.CurrentRoom.PlayerCount;

        try {
            scoreText.text = player_count.ToString();
        }// make it a string to output to the Text object
        catch 
        {
            Debug.Log("Buscando el objeto");
        }

    }

    //Que va ocurrir cuando nos unamos al cuarto
    /*public override void OnJoinedRoom()
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
    }*/


    public void Join()
    {

        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Se unio a la sala exitosamente");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        //Debug.Log(PhotonNetwork.CurrentRoom.numrt)
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Error !  No se pudo usar");
        Join();
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Erro ! Sala no creada");
        OnJoinedLobby();
        //base.OnCreateRoomFailed(returnCode, "Error no se pudo crear la sala");
    }

}