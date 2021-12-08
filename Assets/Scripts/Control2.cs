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
    // private Thread mThread;

    // private string connectionIp = "127.0.0.1";
    // private int connectionpPort = 50002;

    // private IPAddress localAdd;
    // private TcpClient client;
    //
    // private TcpListener listener;

    // private bool closePython;

    private bool statusChange = false;
    // private bool listenCamera = true;
    // private bool closeCamera = true;

    void Start()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
        ManagerGame = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();


        try
        {
            keywordRecognizerSpeech = GameObject.FindGameObjectWithTag("tagAudio").GetComponent<ControlAudio>();
            keywordRecognizerSpeech.keywordRecognizer.Stop();
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
                keywordRecognizerSpeech = GameObject.FindGameObjectWithTag("tagAudio").GetComponent<ControlAudio>();
                keywordRecognizerSpeech.keywordRecognizer.Start();
                Debug.Log("Se encontro el Diccionario del U 2");
            }
            catch (NullReferenceException)
            {
                Debug.Log("No se encontro el obejto");
            }
        }
        
        
        float travelDistance =
            GameManager.singleton.EntireDistance - GameManager.singleton.DistanceLeft;
        float value = travelDistance / GameManager.singleton.EntireDistance;

        if (value >= 0.5 && !statusChange)
        {
            statusChange = true;
            Debug.Log("Cambio de Controles del jugador 2");
        }
        
        
        //---------Activate o Deactivate Dictionary of Events with voice-------
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