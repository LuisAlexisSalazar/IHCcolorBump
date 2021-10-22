using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesControl : MonoBehaviour
{
    // Start is called before the first fra
    // me update
    public void LoadScene(string sceneName)
    {
        //StartCoroutine(ExampleCoroutine());
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator ExampleCoroutine()
    {
        
        yield return new WaitForSeconds(7);

    }
}
