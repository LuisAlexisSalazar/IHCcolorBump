using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class BallController : MonoBehaviour
{
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


    // Update is called once per frame
    void Update()
    {
        GameManager.singleton.StartGame();
        float mh = Input.GetAxis("Horizontal");
        // float mv = Input.GetAxis("Vertical");
        //[0..0.5..1]
        // Debug.Log("H"+mh);
        // Debug.Log("V:"+mh);
        
    
        Vector3 force = new Vector3(mh, 0, 1)*thrust;
        // Debug.Log(force);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(force);
            Jump();
        }
        else
            rb.AddForce(force);
    }

    public void Jump()
    {
        rb.AddForce(0,15,0,ForceMode.Impulse);
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