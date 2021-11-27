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
    public float moveX = 0;

    [SerializeField]
    //objeto que controla la posición
    public Rigidbody rb;


    //!Eliminar para que no tengamos parecdes invisibles
    [SerializeField] private float wallDistance = 5f;
    [SerializeField] private float minCamDistance = 3f;

    public bool closePython;
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
    public Vector2 dataFaceAcceleration = Vector2.zero;

    
 

    void Start()
    {
        speedUp = false;
    }


    // Update is called once per frame
    void Update()
    {
        GameManager.singleton.StartGame();

        
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
        Vector3 force = new Vector3(moveX, 0, 1) * thrust;

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
            
            
            closePython = true;
            SceneManager.LoadScene("DeadMenu");
            // running = false;
        }
    }
}