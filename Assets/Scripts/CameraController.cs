using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject astroid;
    public Transform farRight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = astroid.transform.position;
		if (pos.x > 0 && pos.x < farRight.position.x)
        {
            transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
        }
	}
}
