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

    private void Start()
    {
        posInitial = transform.position;
        relative = transform.position - Ball.transform.position;
    }

    private void LateUpdate()
    {
        if (GameManager.singleton.GameStarted)
        {
            Vector3 newPosition = Ball.transform.position + relative;
            transform.position = new Vector3(posInitial.x,newPosition.y,newPosition.z);
            
        }
    }
}