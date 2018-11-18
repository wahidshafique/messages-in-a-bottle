using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour {
    [SerializeField]
    private GameObject note;
    public float noteWaitInterval = 0.5f;

	// Use this for initialization
	void Start () {
            StartCoroutine(DelayInstantiate());
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void regenerate (GameObject msg) {
        msg.transform.position = transform.position;
    }

    IEnumerator DelayInstantiate() {
        yield return new WaitUntil(() => GetFirebaseData.allNotes != null);
        Note[] gotNotes = GetFirebaseData.allNotes;
        
        for (int i = 0; i < gotNotes.Length; i++) {
           GameObject instantiatedObj = Instantiate(note, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            instantiatedObj.name = i.ToString();

            MessageScript msgScript = instantiatedObj.GetComponent<MessageScript>();

            msgScript.message = gotNotes[i].note;
            msgScript.read = gotNotes[i].read;
            msgScript.index = i;
            yield return new WaitForSeconds(noteWaitInterval);
        }
    }
}
