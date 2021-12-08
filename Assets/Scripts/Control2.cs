using System;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Windows.Speech;

public class Control2 : MonoBehaviour
{
    public bool flagKeyboard = false;
    public int UserID = 2;

    private BallController Ball;
    private GameManager ManagerGame;

    //VOICE
    ControlAudio keywordRecognizerSpeech;
    private bool statusFindTagAudio = false;

    //!Conexción con python
    private Thread mThread;

    private string connectionIp = "127.0.0.1";
    private int connectionpPort = 50002;

    private IPAddress localAdd;
    private TcpClient client;

    private TcpListener listener;


    private bool running;
    private bool statusChange = false;
    private bool listenCamera = true;
    private bool closeCamera = false;

    void Start()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
        ManagerGame = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

        try
        {
            keywordRecognizerSpeech = GameObject.FindGameObjectWithTag("tagAudio")
                .GetComponent<ControlAudio>();
            keywordRecognizerSpeech.keywordRecognizer.Start();
            statusFindTagAudio = true;
            Debug.Log("Se encontro el Diccionario del U 2");
        }
        catch (NullReferenceException)
        {
            Debug.Log("No se encontro el obejto de audio");
        }
    }

    void Update()
    {
        if (!statusFindTagAudio)
        {
            try
            {
                keywordRecognizerSpeech = GameObject.FindGameObjectWithTag("tagAudio")
                    .GetComponent<ControlAudio>();
                keywordRecognizerSpeech.keywordRecognizer.Start();
                Debug.Log("Se encontro el Diccionario del U 2");
                statusFindTagAudio = true;
            }
            catch (NullReferenceException)
            {
                Debug.Log("No se encontro el obejto");
            }
        }


        float travelDistance =
            GameManager.singleton.EntireDistance - GameManager.singleton.DistanceLeft;
        float value = travelDistance / GameManager.singleton.EntireDistance;


        //?---------Only Switch de Controles-------
        //---------Activate o Deactivate Dictionary of Events with voice-------
        if (statusChange)
        {
            Ball.moveX = flagKeyboard ? Input.GetAxis("Horizontal") : Ball.dataFaceAcceleration.x;
        }
        else
        {
            // if (value >= 0.5 && !statusChange)
            if (value >= 0.1 && !statusChange)
            {
                statusChange = true;
                Debug.Log("Cambio de Controles del jugador 2");

                keywordRecognizerSpeech.keywordRecognizer.Stop();

                ThreadStart ts = new ThreadStart(GetInfo);
                mThread = new Thread(ts);
                mThread.Start();
            }
            
            
            //--Control Diccionario---
            switch (keywordRecognizerSpeech.keywordRecognizer.IsRunning)
            {
                case true when Input.GetKeyDown(KeyCode.Space):
                    keywordRecognizerSpeech.keywordRecognizer.Stop();
                    Debug.Log("Silenciar");
                    break;
                case false when Input.GetKeyDown(KeyCode.Space):
                    keywordRecognizerSpeech.keywordRecognizer.Start();
                    Debug.Log("Iniciar Microfono");
                    break;
            }
        }
    }

    //----------Fuciones para recibir datos de python-------

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIp);
        listener = new TcpListener(IPAddress.Any, connectionpPort);
        listener.Start();
        client = listener.AcceptTcpClient();
        running = true;
        Ball.closePython = false;
        while (running)
            ReceiveData();


        listener.Stop();
        client.Close();
    }

    void ReceiveData()
    {
        NetworkStream networkStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //Recibir datos desde el host
        int byteRead = networkStream.Read(buffer, 0, client.ReceiveBufferSize);
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, byteRead);

        if (dataReceived != null)
        {
            Ball.dataFaceAcceleration = StringToArray(dataReceived);
            // Debug.Log("Nueva Aceleración: " + Ball.dataFaceAcceleration.y);
        }

        if (Ball.closePython || closeCamera)
        {
            byte[] myWriteBuffer = Encoding.ASCII.GetBytes("close");
            networkStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
            running = false;
        }
        else
        {
            byte[] myWriteBuffer = Encoding.ASCII.GetBytes("keep");
            networkStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
        }
    }

    public static Vector2 StringToArray(string wholeStringArray)
    {
        string[] sArray = wholeStringArray.Split(',');
        Vector2 result = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));
        return result;
    }
}