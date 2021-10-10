using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play_audio : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject audioj;
    public void DropAudio()
    {
        Instantiate(audioj, transform.position, transform.rotation);
    }
    
}
