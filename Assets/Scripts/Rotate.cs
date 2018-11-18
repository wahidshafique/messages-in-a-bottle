using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    float xRotation = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        xRotation += 0.01f;
        transform.eulerAngles = new Vector3(17.764f, xRotation, transform.rotation.z);
    }
}
