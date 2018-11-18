using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret : MonoBehaviour {
    public Sprite altSprite;
    private SpriteRenderer m_sprenderer;
	// Use this for initialization
	void Start () {
        m_sprenderer = GetComponent<SpriteRenderer>();
	}
	
    void OnMouseDown() {
        m_sprenderer.sprite = altSprite;
    }
}
