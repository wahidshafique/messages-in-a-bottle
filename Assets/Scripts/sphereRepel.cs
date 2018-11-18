using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereRepel : MonoBehaviour {
    private NoteGenerator noteGen;
    private AudioSource aud;
    private bool isOn = false;
    void Start() {
        aud = GetComponent<AudioSource>();
        noteGen = GameObject.FindGameObjectWithTag("gen").GetComponent<NoteGenerator>();
    }


    void OnMouseDown() {
        if (!isOn) {
            aud.Play();
            isOn = true;
        } else {
            aud.Stop();
        }
    }

    void OnMouseDrag() {

    }

    void OnMouseUp() {

    }
}
