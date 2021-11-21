using System;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlSecondPlater : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    //objeto que controla la posición
    private Rigidbody rb;


    void Start()
    {
        //SUBE,ARRIBA,SALTA
        keywords.Add("salta", Jump);
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        rb = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);
        keywords[args.text].Invoke();
    }


    public void Jump()
    {
        Debug.Log("JUMP 2");
        rb.AddForce(0, 15, 0, ForceMode.Impulse);
    }
}


// [SerializeField] [Range(1, 5)] private float thrust = 1f;
//
// [SerializeField]
//
// //objeto que controla la posición
// private Rigidbody rb;
//
// [SerializeField] private float wallDistance = 5f;
// [SerializeField] private float minCamDistance = 3f;
//
// KeywordRecognizer keywordRecognizer;
// Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
//
//
// //SpeedUp
// private bool speedUp;
// private float time = 0f;
// private bool justOne = false;
//
// //BULLET
// private GameObject bullet;
//
// //BULLETFORCE
// private float shootForce;
//
// //GRAPHICS
//
// public GameObject muzzleFlash;
//
// //BOOLS
// private bool shooting;
//
// //CAMARA
// private Camera fcam;
// private Transform attackPoint;
//
// //VOICE
//
//
// //LA BALA
// //? la bvala?
// private GameObject cam;
//
// //public Transform cam;
// //lic LayerMask objDestro;
//
// //!Conexción con python
// private Thread mThread;
// private string connectionIp = "127.0.0.1";
// private int connectionpPort = 50001;
// private IPAddress localAdd;
// private TcpClient client;
// private TcpListener listener;
// private Vector2 dataFaceAcceleration = Vector2.zero;
// private bool running;
// private bool close_python;
//
// void Start()
// {
//     speedUp = false;
//     //SUBE,ARRIBA,SALTA
//     keywords.Add("sube", Jump);
//     keywords.Add("dispara", fire);
//     keywords.Add("turbo", Nitro);
//     keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
//     keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
//     keywordRecognizer.Start();
//
//     //!Buscar los objetos ya creados  para el RigiBody
//     rb = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
//     fcam = GameObject.FindGameObjectWithTag("ShootCam").GetComponent<Camera>();
//     attackPoint = GameObject.FindGameObjectWithTag("AttackPoint").transform;
//
//     // Conexión a python con otro thread
//     ThreadStart ts = new ThreadStart(GetInfo);
//     mThread = new Thread(ts);
//     mThread.Start();
// }
//
// private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
// {
//     Debug.Log(args.text);
//     keywords[args.text].Invoke();
// }
//
//
// // Update is called once per frame
// void Update()
// {
//     // float move_x = Input.GetAxis("Horizontal");
//     //?usar detector de Cara
//     float move_x = dataFaceAcceleration.x;
//
//     if (justOne && speedUp)
//     {
//         thrust += 5f;
//         justOne = false;
//     }
//
//     if (dataFaceAcceleration.y != 0)
//         thrust = dataFaceAcceleration.y * 5;
//
//     Vector3 force = new Vector3(move_x, 0, 1) * thrust;
//
//     rb.AddForce(force);
//
//     //------------
//     if (speedUp && !justOne)
//     {
//         time += Time.deltaTime;
//         if (0.9 < time)
//         {
//             //Debug.Log(time);
//             //Debug.Log(thrust);
//             thrust -= 5f;
//             speedUp = false;
//             time = 0;
//         }
//     }
// }
//
//
// public void Nitro()
// {
//     Debug.Log("Nitro 2");
//     speedUp = true;
//     justOne = true;
// }
//
// public void Jump()
// {
//     Debug.Log("JUMP 2");
//     rb.AddForce(0, 15, 0, ForceMode.Impulse);
// }
//
// private void fire()
// {
//     Debug.Log("FIRE 3");
//     Shoot();
// }
//
// private void Shoot()
// {
//     //LA DIRECCION DE LA CAMARA
//     Ray ray = fcam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
//     RaycastHit hit;
//     Vector3 targetPoint;
//     if (Physics.Raycast(ray, out hit))
//     {
//         targetPoint = hit.point;
//     }
//     else
//     {
//         targetPoint = ray.GetPoint(75);
//     }
//
//
//     Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
//
//     GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
//
//     //FORCE TO THE BULLET
//     Debug.Log(directionWithoutSpread);
//
//     currentBullet.GetComponent<Rigidbody>()
//         .AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
//     GameObject.Destroy(currentBullet, 3f);
//
//     if (muzzleFlash != null)
//     {
//         Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
//     }
// }
//
// public static Vector2 StringToArray(string WholeStringArray)
// {
//     string[] sArray = WholeStringArray.Split(',');
//     Vector2 result = new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));
//     return result;
// }
//
// void GetInfo()
// {
//     localAdd = IPAddress.Parse(connectionIp);
//     listener = new TcpListener(IPAddress.Any, connectionpPort);
//     listener.Start();
//     client = listener.AcceptTcpClient();
//     running = true;
//     close_python = false;
//     while (running)
//         ReceiveData();
//
//
//     listener.Stop();
//     client.Close();
// }
//
// void ReceiveData()
// {
//     NetworkStream networkStream = client.GetStream();
//     byte[] buffer = new byte[client.ReceiveBufferSize];
//
//     //Recibir datos desde el host
//     int byteRead = networkStream.Read(buffer, 0, client.ReceiveBufferSize);
//     string dataReceived = Encoding.UTF8.GetString(buffer, 0, byteRead);
//
//     if (dataReceived != null)
//     {
//         dataFaceAcceleration = StringToArray(dataReceived);
//         // Debug.Log("Nueva Aceleración: "+dataFaceAcceleration.y);
//     }
//
//     if (close_python)
//     {
//         byte[] myWriteBuffer = Encoding.ASCII.GetBytes("close");
//         networkStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
//         running = false;
//     }
//     else
//     {
//         byte[] myWriteBuffer = Encoding.ASCII.GetBytes("keep");
//         networkStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
//     }
// }