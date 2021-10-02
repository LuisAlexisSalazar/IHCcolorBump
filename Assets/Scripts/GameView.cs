using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private Image fillBarrProgress;

    private float lastValue;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.singleton.GameStarted)
            return;

        float travelDistance =
            GameManager.singleton.EntireDistance - GameManager.singleton.DistanceLeft;
        float value = travelDistance / GameManager.singleton.EntireDistance;

        if (GameManager.singleton.gameObject && value < lastValue)
            return;
        
        //Aumentar la barra de progreso
        fillBarrProgress.fillAmount =
            Mathf.Lerp(fillBarrProgress.fillAmount, value, 5 * Time.deltaTime);
        
        // Debug.Log("Progreso:"+fillBarrProgress.fillAmount);
        lastValue = value;
        
        
    }
}