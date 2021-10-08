using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Goal : MonoBehaviour
{
    string prefijNameScene = "GameLevel";
    [SerializeField] private int indexLevel = 1;

    // private GameObject[] Ball;

    // https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html
    //Solo es llamado cuando colisiona y activado de un objeto BoxCollider->is Trigger
    private void OnTriggerEnter(Collider other)
    {
        //Se podria usar el tag
        BallController ball = other.GetComponent<BallController>();

        // Ball =  GameObject.FindGameObjectsWithTag ("Ball"); 
        // Debug.Log("Goal: " + GameManager.singleton.GameEnded);

        if (!ball || GameManager.singleton.GameEnded)
            return;

        Debug.Log(other);
        Debug.Log("Objetivo tocado");

        //!Reiniciar el Juego
        // GameManager.singleton.EndGame(true);

        //!Pasar de Nivel
        SceneManager.LoadScene(prefijNameScene + indexLevel.ToString());
    }
}