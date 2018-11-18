using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCamScript : MonoBehaviour {
    private GameObject mainCam;
    void Awake() {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update() {
        fastForward();
    }

    void fastForward() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            gameObject.GetComponent<Animator>().speed *= 2;
        }
    }

    public void destroySelf() {
        //notify the event script
        mainCam.GetComponent<eventScript>().canActivateFirstPerson = true;
        print("destroying");
        Destroy(this.gameObject);
    }
}
