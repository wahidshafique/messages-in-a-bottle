using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitBounds : MonoBehaviour {
    private NoteGenerator noteGen;

    void Start() {
        noteGen = GameObject.FindGameObjectWithTag("gen").GetComponent<NoteGenerator>();
		
	}
	
    void OnTriggerEnter2D(Collider2D other) {
        noteGen.regenerate(other.gameObject);
        print("gi");
    }
}
