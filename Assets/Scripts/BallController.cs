using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine;
//WINDOWS SPEECH



public class BallController : MonoBehaviour
{

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

    /// <summary>
    /// --------------------------------------------------------------------------------------
    /// </summary>


    //LA BALA
    public GameObject cam;

    //public Transform cam;
   //lic LayerMask objDestro;

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
        float mh = Input.GetAxis("Horizontal");
        // float mv = Input.GetAxis("Vertical");
        //[0..0.5..1]
        // Debug.Log("H"+mh);
        // Debug.Log("V:"+mh);
        

  
        if(justOne && speedUp)
        {
            thrust += 5f;
            justOne = false;
        }

        //Debug.Log(thrust);
        Vector3 force = new Vector3(mh, 0, 1) * thrust;
        rb.AddForce(force);

        if(speedUp && !justOne)
        {
            time += Time.deltaTime;
            
            if (0.9 < time)
            {
                //Debug.Log(time);
                //Debug.Log(thrust);
                thrust -= 5f;
                fSpeed = 0;
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
        rb.AddForce(0,15,0,ForceMode.Impulse);
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
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
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
        Debug.Log(collision.gameObject.tag);

        if (GameManager.singleton.GameEnded)
            return;

        if (collision.gameObject.tag == "Death")
            GameManager.singleton.EndGame(false);
    }
}