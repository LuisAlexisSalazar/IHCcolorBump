using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine;
using TMPro;
//WINDOWS SPEECH

public class FPS_A : MonoBehaviour
{

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

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start()
    {
        keywords.Add("fire", fire);
     
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }


    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);
        keywords[args.text].Invoke();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Shoot();
        }
    }

    private void fire()
    {
        Debug.Log("FIRE");
        Shoot();
    }

    private void Shoot()
    {
        //LA DIRECCION DE LA CAMARA
        Ray ray = fcam.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
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

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);

        if(muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

    }
}