using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventScript : MonoBehaviour {
    public bool canActivateFirstPerson = false;

    private GameObject fpsView;
    private Camera fpsCam;
    private UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController fpsScript;
    private UnityStandardAssets.Characters.FirstPerson.HeadBob fpsHeadBob;
    private AudioSource lune;
    private GameObject musicObj;

    void Awake() {
        fpsView = GameObject.FindGameObjectWithTag("Player");
        fpsCam = fpsView.gameObject.GetComponentInChildren<Camera>();
        fpsScript = fpsView.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        fpsHeadBob = fpsView.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.HeadBob>();
        musicObj = GameObject.Find("music");
        lune = musicObj.GetComponent<AudioSource>();
    }

    void Update() {
        activatePlayer();
    }

    void activatePlayer() {
        if (canActivateFirstPerson) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                fpsCam.enabled = true;
                fpsScript.enabled = true;
                fpsHeadBob.enabled = true;
                Destroy(musicObj);
                destroySelf();
            }
        }
    }

    public void destroySelf() {
        print("destroying");
        Destroy(this.gameObject);
    }
}
