using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // public Vector3 offset = new Vector3(-0.3f, 5.5f, -0.8f);
    public GameObject Ball;
    private Vector3 posInitial;
    private Vector3 relative;
    
    //--
    private Transform target=null;
    //--
    private void Start()
    {
        posInitial = transform.position;

        //-----
        // target = GameObject.FindGameObjectWithTag("Ball").transform;
        //-----

        relative = transform.position - Ball.transform.position;
      

        // relative = transform.position - target.transform.position;
        
    }

    private void LateUpdate()
    {
        
        if (target == null)
        {
            try
            {
                target = GameObject.FindGameObjectWithTag("Ball").transform;
            }
            catch (NullReferenceException)
            {
                target = null;
            }
        }
        //--
        // target = GameObject.FindGameObjectWithTag("Ball").transform;
        //--
        
        if (GameManager.singleton.GameStarted)
        {
            // Vector3 newPosition = Ball.transform.position + relative;
            
            //---
            Vector3 newPosition = target.transform.position + relative;
            //--
            
            transform.position = new Vector3(posInitial.x, newPosition.y, newPosition.z);
        }
    }
}