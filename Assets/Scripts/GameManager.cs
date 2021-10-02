using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public bool GameStarted { get; private set; }
    public bool GameEnded { get; private set; }

    [SerializeField] private float slowMotionFactor = .1f;

    //?Para la barra de progreso
    //Posicion inicial,posicion final y posición de la bola
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform goalTransform;
    [SerializeField] private BallController ball;

    public float EntireDistance { get; private set; }
    public float DistanceLeft { get; private set; }

    private void Start()
    {
        EntireDistance = goalTransform.position.z - startTransform.position.z;
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }


    public void StartGame()
    {
        GameStarted = true;
        // Debug.Log("Inicio del Juego");
    }

    public void EndGame(bool win)
    {
        GameEnded = true;
        Debug.Log("Fin del Juego");

        if (!win)
        {
            //Resetear el juego
            Invoke("RestarGame", slowMotionFactor);
            Time.timeScale = slowMotionFactor;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            Debug.Log("Fin del Juego Ganaste");
            Invoke("RestarGame", 2);
        }
    }

    public void RestarGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Update()
    {
        DistanceLeft = Vector3.Distance(ball.transform.position, goalTransform.position);
        if (DistanceLeft > EntireDistance)
            DistanceLeft = EntireDistance;
        if (ball.transform.position.z > goalTransform.transform.position.z)
            DistanceLeft = 0;

        // Debug.Log(
        //     "Distancia Recorrido es " + DistanceLeft + " Distance entera es " + EntireDistance);
    }
}