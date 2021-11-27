using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;

public class controlOne : MonoBehaviour
{
    ControlAudio keywordRecognizerSpeech;

    // Start is called before the first frame update
    void Start()
    {
        keywordRecognizerSpeech =
            GameObject.FindGameObjectWithTag("tagAudio").GetComponent<ControlAudio>();
        keywordRecognizerSpeech.keywordRecognizer.Stop();
    }

    void Update()
    {
        if (keywordRecognizerSpeech.keywordRecognizer.IsRunning && Input.GetKeyDown(KeyCode.Space))
        {
            keywordRecognizerSpeech.keywordRecognizer.Stop();
            Debug.Log("Silenciar");
        }

        else

        {
            if (!keywordRecognizerSpeech.keywordRecognizer.IsRunning &&
                Input.GetKeyDown(KeyCode.Space))
            {
                keywordRecognizerSpeech.keywordRecognizer.Start();
                Debug.Log("Iniciar Microfono");
            }
        }
    }
}