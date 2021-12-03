using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class playerCounts : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI scoreText;  // public if you want to drag your text object in there manually
    int player_Counter;

   // void Start()
    //{
      //  scoreText = GetComponent<Text>();  // if you want to reference it by code - tag it if you have several texts
    //}

    void Update()
    {
        //player_Counter = PhotonNetwork.CurrentRoom.PlayerCount;
        player_Counter = 1;
        scoreText.text = player_Counter.ToString();  // make it a string to output to the Text object
    }
    
    
}
