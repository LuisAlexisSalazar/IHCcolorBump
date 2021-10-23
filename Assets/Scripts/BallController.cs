using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine.Windows.Speech;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine;

//WINDOWS SPEECH


public class BallController : MonoBehaviour
{
    [SerializeField] [Range(1, 5)]
    //!Remmplazar por los dedos de mano (OpenCV)
    //influencia la velocidad de la bola
    private float thrust = 1f;

    [SerializeField]
    //objeto que controla la posición
    private Rigidbody rb;


    //!Eliminar para que no tengamos parecdes invisibles
    [SerializeField] private float wallDistance = 5f;
    [SerializeField] private float minCamDistance = 3f;

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();


    //SpeedUp
    public bool speedUp;
    public float time = 0f;
    public float fSpeed = 0;
    public bool justOne = false;

    //BULLET
    public GameObject bullet;

    //BULLETFORCE
    public float shootForce;

    /*//GUN STATS
    public float timeBetweeShooting;*/

    //GRAPHICS

    public GameObject muzzleFlash;

    //BOOLS
    bool shooting;

    //CAMARA
    public Camera fcam;
    public Transform attackPoint;
    public bool allowInvoke = true;
    public LayerMask objDestro;

    //VOICE


    //LA BALA
    public GameObject cam;

    //public Transform cam;
    //lic LayerMask objDestro;

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

    void Start()
    {
        speedUp = false;
        //SUBE,ARRIBA,SALTA
        keywords.Add("sube", Jump);
        keywords.Add("dispara", fire);
        keywords.Add("turbo", Nitro);
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        // Conexión a python con otro thread
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);
        keywords[args.text].Invoke();
    }


    // Update is called once per frame
    void Update()
    {
        GameManager.singleton.StartGame();
        // float move_x = Input.GetAxis("Horizontal");
        //?usar detector de Cara
        float move_x = dataFaceAcceleration.x;

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


    public void Nitro()
    {
        Debug.Log("Nitro");
        speedUp = true;
        justOne = true;
    }

    public void Jump()
    {
        Debug.Log("JUMP");
        rb.AddForce(0, 15, 0, ForceMode.Impulse);
    }

    private void fire()
    {
        Debug.Log("FIRE");
        Shoot();
    }

    private void Shoot()
    {
        //LA DIRECCION DE LA CAMARA
        Ray ray = fcam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }


        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //FORCE TO THE BULLET
        Debug.Log(directionWithoutSpread);
        /*if(directionWithoutSpread.z > 5)
        {
            directionWithoutSpread = new Vector3(directionWithoutSpread.x, directionWithoutSpread.y, directionWithoutSpread.z % 5);
        }
        Vector3 unit = new Vector3(0.0f, 0.0f, 1f);*/
        currentBullet.GetComponent<Rigidbody>()
            .AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        GameObject.Destroy(currentBullet, 3f);

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
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

        if (collision.gameObject.tag == "Death")
        {
            // Debug.Log(collision.gameObject.tag);
            GameManager.singleton.EndGame(false);
            close_python = true;
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