using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_idle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.Rotate(1.5f, 0.0f, 0.0f, Space.Self);
        this.transform.Rotate(0.0f, 0.3f, 0.5f, Space.Self);
        //this.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
    }
}