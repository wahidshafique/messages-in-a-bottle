using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousNote : MonoBehaviour {
    [SerializeField, HideInInspector]
    private GameObject previousNote = null;

    public void setPrevious(GameObject current) {
        if (previousNote == null) {
            previousNote = current;
        }
    }

    public GameObject getPrevious() {
        return previousNote;
    }

    public void destroyPrevious(GameObject current) {
        if (previousNote != null && (current.GetInstanceID() != previousNote.GetInstanceID())) {
            previousNote.GetComponent<MessageScript>().triggerRecoiling(() => {
                Destroy(previousNote);
                previousNote = current;
            });
        }
    }
}
