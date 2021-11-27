using System;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class ControlAudio : MonoBehaviour
{
    public KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private PhotonView PV;
    private BallController BallC;


    // Start is called before the first frame update
    void Start()
    {
        // PV = GameObject.FindGameObjectWithTag("Ball").GetComponent<PhotonView>();
        PV = GetComponent<PhotonView>();
        BallC = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
        // PV = GetComponent<PhotonView>();

        BallC.speedUp = false;

        //SUBE,ARRIBA,SALTA
        // keywords.Add("sube", Jump);
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


    public void Nitro()
    {
        PV.RPC("RealityNitro", RpcTarget.All);
    }

    [PunRPC]
    public void RealityNitro()
    {
        Debug.Log("Nitro");
        BallC.speedUp = true;
        BallC.justOne = true;
    }


    void Jump()
    {
        PV.RPC("RealityJump", RpcTarget.All);
    }

    [PunRPC]
    void RealityJump()
    {
        Debug.Log("JUMP");
        BallC.rb.AddForce(0, 15, 0, ForceMode.Impulse);
        // rb.AddForce(0, 15, 0, ForceMode.Impulse);
    }

    private void fire()
    {
        Debug.Log("FIRE");
        PV.RPC("Shoot", RpcTarget.All);
        // Shoot();
    }

    [PunRPC]
    private void Shoot()
    {
        if (BallC.fcam == null)
        {
            try
            {
                BallC.fcam = GameObject.FindGameObjectWithTag("ShootCam")
                    .GetComponent<Camera>() as Camera;
            }
            catch (NullReferenceException)
            {
                BallC.fcam = null;
                return;
            }
        }

        if (BallC.attackPoint == null)
        {
            try
            {
                BallC.attackPoint = GameObject.FindGameObjectWithTag("AttackPoint").transform;
            }
            catch (NullReferenceException)
            {
                BallC.attackPoint = null;
                return;
            }
        }


        //LA DIRECCION DE LA CAMARA
        Ray ray = BallC.fcam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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


        Vector3 directionWithoutSpread = targetPoint - BallC.attackPoint.position;

        GameObject currentBullet =
            Instantiate(BallC.bullet, BallC.attackPoint.position, Quaternion.identity);

        //FORCE TO THE BULLET
        Debug.Log(directionWithoutSpread);
        /*if(directionWithoutSpread.z > 5)
        {
            directionWithoutSpread = new Vector3(directionWithoutSpread.x, directionWithoutSpread.y, directionWithoutSpread.z % 5);
        }
        Vector3 unit = new Vector3(0.0f, 0.0f, 1f);*/
        currentBullet.GetComponent<Rigidbody>()
            .AddForce(directionWithoutSpread.normalized * BallC.shootForce, ForceMode.Impulse);
        GameObject.Destroy(currentBullet, 3f);

        if (BallC.muzzleFlash != null)
        {
            Instantiate(BallC.muzzleFlash, BallC.attackPoint.position, Quaternion.identity);
        }
    }
}