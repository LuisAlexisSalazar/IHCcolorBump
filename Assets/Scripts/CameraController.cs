using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // public Vector3 offset = new Vector3(-0.3f, 5.5f, -0.8f);
    public GameObject Ball = null;
    private Vector3 posInitial;
    private Vector3 relative;

    private void Start()
    {
        posInitial = transform.position;
    }

    private void LateUpdate()
    {
        if (Ball == null)
        {
            try
            {
                Ball = GameObject.FindGameObjectWithTag("Ball");
                relative = transform.position - Ball.transform.position;
            }
            catch (NullReferenceException)
            {
                Ball = null;
                return;
            }
        }


        if (!GameManager.singleton.GameStarted) return;

        Vector3 newPosition = Ball.transform.position + relative;
        transform.position = new Vector3(posInitial.x, newPosition.y, newPosition.z);
    }

    // IEnumerator setConfig()
    // {
    //     while (target != null)
    //     {
    //         try
    //         {
    //             target = GameObject.FindGameObjectWithTag("Ball").transform;
    //             relative = transform.position - target.position;
    //         }
    //         catch (NullReferenceException)
    //         {
    //             target = null;
    //         }
    //     }
    //
    //     yield return null;
    // }
}