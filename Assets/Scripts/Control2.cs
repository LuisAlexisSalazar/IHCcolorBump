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


    //VOICE
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


        keywordRecognizerSpeech =
            GameObject.FindGameObjectWithTag("tagAudio").GetComponent<ControlAudio>();
        keywordRecognizerSpeech.keywordRecognizer.Stop();
    }

    void Update()
    {
        float travelDistance =
            GameManager.singleton.EntireDistance - GameManager.singleton.DistanceLeft;
        float value = travelDistance / GameManager.singleton.EntireDistance;

        if (value >= 0.5 && !statusChange)
        {
            statusChange = true;
            Debug.Log("Cambio de Controles del jugador 2");
        }

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