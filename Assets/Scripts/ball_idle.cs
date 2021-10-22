using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ball_idle : MonoBehaviour
{

    //IDLE BALL
    string sceneName;
    float delaytime = 2f;
    public void JumpBall(string sceneN)
    {
        sceneName = sceneN;
        
        this.transform.LeanMoveLocal(new Vector2(-0.071f,-7.09f), 4f).setEaseOutQuart();
        Invoke("LoadScene", delaytime);
    }
    public void Tutorial(string sceneN)
    {
        sceneName = sceneN;
        this.transform.LeanMoveLocal(new Vector3(-0.071f,0.212f,13.0f), 3f).setEaseOutQuart().setLoopPingPong();
        Invoke("LoadScene", delaytime);
    }

    public void DropBall(string sceneN)
    {
        sceneName = sceneN;
        this.transform.LeanMoveLocal(new Vector2(-0.071f, 7.09f), 4f).setEaseOutQuart();
        Invoke("LoadScene", delaytime);
    }

    public void QuitS()
    {
        this.transform.LeanMoveLocal(new Vector2(-0.071f, -7.09f), 4f).setEaseOutQuart();
        Application.Quit();
        Debug.Log("QUIT");
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.Rotate(1.5f, 0.0f, 0.0f, Space.Self);
        this.transform.Rotate(0.0f, 0.3f, 0.5f, Space.Self);
        //this.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
    }


    public void LoadScene()
    {
        //StartCoroutine(ExampleCoroutine());
        SceneManager.LoadScene(sceneName);
    }


}
