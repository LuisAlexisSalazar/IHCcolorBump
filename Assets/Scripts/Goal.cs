using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // private GameObject[] Ball;

    // https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html
    //Solo es llamado cuando colisiona y activado de un objeto BoxCollider->is Trigger
    private void OnTriggerEnter(Collider other)
    {
        //Se podria usar el tag
        BallController ball = other.GetComponent<BallController>();

        // Ball =  GameObject.FindGameObjectsWithTag ("Ball"); 
        Debug.Log("Goal: " + GameManager.singleton.GameEnded);
        
        if (!ball || GameManager.singleton.GameEnded)
            return;
        Debug.Log(other);
        Debug.Log("Objetivo tocado");
        GameManager.singleton.EndGame(true);
    }
}