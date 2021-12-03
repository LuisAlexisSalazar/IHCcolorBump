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
    private bool existBall2 = false;
    TextMeshProUGUI scoreText;
    /*TextMeshProUGUI nameText;
    image;*/
    private int player_count;
    bool two_players = false;
    bool isDestroy = false;

    void Start()
    {
        //Conexión al servidor
        PhotonNetwork.ConnectUsingSettings();
    }

    //Conectar al servidor
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //Unirse al lobby
    public override void OnJoinedLobby()
    {
        //Unirse a un room ya creado o creara el room si no existe
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
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
            existBall = true;
        if(existBall)
        {
            scoreText = GameObject.FindGameObjectWithTag("text1").GetComponent<TextMeshProUGUI>();
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            existBall2 = true;

        if (existBall2)
        {
            Debug.Log("Jugador 2 debe controlar balon 1");
            // GameObject Control2 = 
            PhotonNetwork.Instantiate("control2",
                new Vector3(-0.2f, -0.15f, 2.41f),
                Quaternion.identity);

        }
        else
        {
            Debug.Log("Jugador 1 Creación del Balon");
            // Objeto de la carpeta resources , dodne se instanciara
            PhotonNetwork.Instantiate("SplitMetalBall", new Vector3(-0.2f, -0.15f, 2.41f),
                Quaternion.identity);

            PhotonNetwork.Instantiate("controlAudio", new Vector3(-0.2f, -0.15f, 2.41f),
                Quaternion.identity);

            // GameObject Control1 =
            PhotonNetwork.Instantiate("control",
                new Vector3(-0.2f, -0.15f, 2.41f), Quaternion.identity);

        }
    }

    void Update()
    {

        try 
        {
            player_count = PhotonNetwork.CurrentRoom.PlayerCount;
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                two_players = true;
            }
        }
        catch
        {
            Debug.Log("Player count");
        }
        //player_count = PhotonNetwork.CurrentRoom.PlayerCount;

        if(!isDestroy)
        {
            try
            {
                scoreText.text = player_count.ToString();
                //Debug.Log("Destroyed");
                //Debug.Log(two_players);
                if (two_players)
                {
                    isDestroy = true;
                }
            }// make it a string to output to the Text object
            catch
            {
                Debug.Log("Buscando el objeto");
            }
        }
        else
        {
            StartCoroutine(waiter());
        }
        

    }
    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(2);
        GameObject title = GameObject.FindGameObjectWithTag("text0");
        Destroy(title);
        //yield return new WaitForSecondsRealtime(1);
        title = GameObject.FindGameObjectWithTag("text00");
        Destroy(title);
        //yield return new WaitForSecondsRealtime(1);
        title = GameObject.FindGameObjectWithTag("text1");
        Destroy(title);
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
        Debug.Log("Error ! Sala no creada");
        OnJoinedLobby();
        //base.OnCreateRoomFailed(returnCode, "Error no se pudo crear la sala");
    }

}