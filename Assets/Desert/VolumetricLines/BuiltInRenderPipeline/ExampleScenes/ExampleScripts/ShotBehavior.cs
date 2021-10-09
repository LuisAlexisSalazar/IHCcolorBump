using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position += transform.forward ;
		transform.position += new Vector3(0.0f,0.0f,0.1f);
	}
}
