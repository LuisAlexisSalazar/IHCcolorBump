using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

//WINDOWS SPEECH


public class BallController : MonoBehaviour
// public class BallController : MonoBehaviourPun
{
    [SerializeField] [Range(1, 5)]
    //!Remmplazar por los dedos de mano (OpenCV)
    //influencia la velocidad de la bola
    private float thrust = 1f;

    public bool flagKeyboard = false;
    private float move_x = 0;

    [SerializeField]
    //objeto que controla la posición
    public Rigidbody rb;


    //!Eliminar para que no tengamos parecdes invisibles
    [SerializeField] private float wallDistance = 5f;
    [SerializeField] private float minCamDistance = 3f;


    //SpeedUp
    public bool speedUp;
    public float time = 0f;
    public bool justOne = false;

    //BULLET
    public GameObject bullet;

    //BULLETFORCE
    public float shootForce;

    //GRAPHICS

    public GameObject muzzleFlash;

    //BOOLS
    bool shooting;

    //CAMARA
    public Camera fcam;
    public Transform attackPoint;


    //VOICE
    //!Conexción con python
    private Thread mThread;
    private string connectionIp = "127.0.0.1";
    private int connectionpPort = 50001;
    private IPAddress localAdd;
    private TcpClient client;
    private TcpListener listener;
    private Vector2 dataFaceAcceleration = Vector2.zero;
    private bool running;
    private bool close_python;
    private PhotonView PV;

    void Start()
    {
        speedUp = false;

        // Conexión a python con otro thread
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }


    // Update is called once per frame
    void Update()
    {
        GameManager.singleton.StartGame();

        if (flagKeyboard)
        {
            move_x = Input.GetAxis("Horizontal");
        }
        else
        {
            //?usar detector de Cara
            move_x = dataFaceAcceleration.x;
        }

        //--------------
        if (justOne && speedUp)
        {
            thrust += 5f;
            justOne = false;
        }

        //--------------


        if (dataFaceAcceleration.y != 0)
            thrust = dataFaceAcceleration.y * 5;

        // Debug.Log("Aceleración: " + thrust);
        Vector3 force = new Vector3(move_x, 0, 1) * thrust;

        rb.AddForce(force);

        //------------
        if (speedUp && !justOne)
        {
            time += Time.deltaTime;
            if (0.9 < time)
            {
                //Debug.Log(time);
                //Debug.Log(thrust);
                thrust -= 5f;
                speedUp = false;
                time = 0;
            }
        }
    }


    //!Paredes invisibles (proximamente Quitar)
    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        if (transform.position.x < -wallDistance)
        {
            pos.x = -wallDistance;
        }
        else if (transform.position.x > wallDistance)
        {
            pos.x = wallDistance;
        }

        //Que la bola no este atras de la camará
        if (transform.position.z < Camera.main.transform.position.z + minCamDistance)
        {
            pos.z = Camera.main.transform.position.z + minCamDistance;
        }

        transform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //?Imprime con todo los objetos que choquemos (incluido el suelo)
        // Debug.Log(collision.ToString());
        // Debug.Log(collision.gameObject.tag);

        if (GameManager.singleton.GameEnded)
            return;

        // if (collision.gameObject.tag == "Death")
        if (collision.gameObject.CompareTag("Death"))
        {
            // Debug.Log(collision.gameObject.tag);
            // GameManager.singleton.EndGame(false);
            close_python = true;
            SceneManager.LoadScene("DeadMenu");
            // running = false;
        }
    }

    public static Vector2 StringToArray(string WholeStringArray)
    {
        string[] sArray = WholeStringArray.Split(',');
        Vector2 result = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));
        return result;
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIp);
        listener = new TcpListener(IPAddress.Any, connectionpPort);
        listener.Start();
        client = listener.AcceptTcpClient();
        running = true;
        close_python = false;
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
            dataFaceAcceleration = StringToArray(dataReceived);
            // Debug.Log("Nueva Aceleración: "+dataFaceAcceleration.y);
        }

        if (close_python)
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
}