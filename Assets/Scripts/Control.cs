using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;

public class Control : MonoBehaviour
{
    public bool flagKeyboard = false;
    public int UserID;


    //VOICE
    ControlAudio keywordRecognizerSpeech;

    private BallController Ball;


    //VOICE
    //!Conexción con python
    private Thread mThread;

    private string connectionIp = "127.0.0.1";

    // private int connectionpPort = 50001;
    public int connectionpPort;
    private IPAddress localAdd;
    private TcpClient client;

    private TcpListener listener;

    // private Vector2 dataFaceAcceleration = Vector2.zero;
    private bool running;
    // private bool closePython;


    void Start()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();

        //---------------------
        // foreach (var device in Microphone.devices)
        // {
        //     // Debug.Log("Name: " + device);
        //     // nameDevice = device.ToString();
        //     Microphone.Start(device.ToString(), true, 50, AudioSettings.outputSampleRate);
        // }
        // Conexión a python con otro thread

        keywordRecognizerSpeech =
            GameObject.FindGameObjectWithTag("tagAudio").GetComponent<ControlAudio>();

        switch (UserID)
        {
            case 2:
                keywordRecognizerSpeech.keywordRecognizer.Stop();
                break;

            case 1:
            {
                // keywordRecognizerSpeech =
                //     GameObject.FindGameObjectWithTag("tagAudio").GetComponent<ControlAudio>();
                // keywordRecognizerSpeech.keywordRecognizer.Stop();

                ThreadStart ts = new ThreadStart(GetInfo);
                mThread = new Thread(ts);
                mThread.Start();
                break;
            }
        }
    }

    void Update()
    {
        /*
        Debug.Log(Microphone.IsRecording(nameDevice));
        if (Microphone.IsRecording(nameDevice) && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Iniciar Microfono");
            Microphone.End(nameDevice);
        }
        else
        {
            if (!Microphone.IsRecording(nameDevice) && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Silenciar");
                Microphone.Start(nameDevice, true, 50, AudioSettings.outputSampleRate);
            }
        }
        */


        if (UserID == 2)
        {
            // !Window.Speech
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


        Ball.moveX = flagKeyboard ? Input.GetAxis("Horizontal") : Ball.dataFaceAcceleration.x;
    }


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
            // Debug.Log("Nueva Aceleración: "+dataFaceAcceleration.y);
        }

        if (Ball.closePython)
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