using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleCam : MonoBehaviour
{
    // public Vector3 offset = new Vector3(-0.3f, 5.5f, -0.8f);
    public GameObject Ball;
    private Vector3 posInitial;
    
    //--
    private Transform target = null;
    //--
    
    private void Start()
    {
        posInitial = transform.position;
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
        
        if (GameManager.singleton.GameStarted)
        {
            Vector3 newPosition = target.transform.position;
            // Vector3 newPosition = Ball.transform.position;
            transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);

        }
    }
}