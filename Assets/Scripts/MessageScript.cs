using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageScript : MonoBehaviour {
    public Transform viewingAnchor;
    public bool coolingDown = false;
    private bool clicked = false;
    private bool animating = false;
    public float scaleFactor = 1;
    public bool isActive = false;
    private Quaternion currentRot;
    private Rigidbody2D rig;
    float randomLilt;
    public GameObject eventHandler;
    private MeshRenderer renderNote;
    private PreviousNote lastNoteRefScript;

    //firebase 
    private GetFirebaseData firebaseScript;
    public string message = "";
    public int index = 0;
    public bool read = false;

    SpriteRenderer m_SpriteRenderer;

    AudioSource touchTing;

    // Use this for initialization
    void Start() {
        touchTing = GetComponent<AudioSource>();
        renderNote = GetComponentInChildren<MeshRenderer>();
        firebaseScript = GameObject.Find("Firebase").GetComponent<GetFirebaseData>();
        lastNoteRefScript = eventHandler.GetComponent<PreviousNote>();
        rig = GetComponent<Rigidbody2D>();
        randomLilt = Random.Range(10, 20);

        if (message != null) {
            fillTextField(message, GetComponentInChildren<TextMesh>());
        } else {
            Destroy(this.gameObject);
            return;
        }
        //GetComponentInChildren<TextMesh>().text = parseText(message);
        currentRot = transform.rotation;


        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        Color color = Color.grey;
        color.a -= 0.3f;
        if (read) {
            m_SpriteRenderer.color = color;
        }
    }

    protected void fillTextField(string text, TextMesh _textMesh) {
        string builder = "";
        _textMesh.text = "";
        float rowLimit = 0.2f; //find the sweet spot    
        string[] parts = text.Split(' ');
        for (int i = 0; i < parts.Length; i++) {
            //Debug.Log(parts[i]);
            _textMesh.text += parts[i] + " ";
            if (_textMesh.GetComponent<Renderer>().bounds.extents.x > rowLimit) {
                _textMesh.text = builder.TrimEnd() + System.Environment.NewLine + parts[i] + " ";
            }
            builder = _textMesh.text;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (clicked) {
            renderNote.enabled = true;
            transform.position = viewingAnchor.position;
            Quaternion normalPos = Quaternion.Euler(0, 0, 180);
            transform.rotation = normalPos;
            Destroy(rig);
            clicked = false;
        }
    }

    private float turningRate = 1f;
    // Rotation we should blend towards.
    private Quaternion _targetRotation;
    // Call this when you want to turn the object smoothly.
    public void SetBlendedEulerAngles(Vector3 angles) {
        _targetRotation = Quaternion.Euler(angles);
    }

    IEnumerator ScaleObject(System.Action cb) {
        //lastNoteRefScript.destroyPrevious(this.gameObject);
        //if (lastNoteRefScript.getPrevious().GetInstanceID() != this.gameObject.GetInstanceID()) {
        //    lastNoteRefScript.getPrevious().GetComponent<MessageScript>().triggerRecoiling(() => {
        //        print("action runing");
        //        lastNoteRefScript.setPrevious(this.gameObject);
        //    });
        //last object destroyed, set this one as valida
        lastNoteRefScript.destroyPrevious(this.gameObject);
        print("scaling");
        isActive = true;
        float scaleDuration = 1;                                //animation duration in seconds
        Vector3 actualScale = transform.localScale;             // scale of the object at the begining of the animation
        Vector3 targetScale = new Vector3(1.0f * scaleFactor, -3.0f * scaleFactor, 7.0f * scaleFactor);     // scale of the object at the end of the animation

        for (float t = 0; t < 1; t += Time.deltaTime / scaleDuration) {
            transform.localScale = Vector3.Lerp(actualScale, targetScale, t);
            //transform.Rotate(Vector3.up * (t * 6));
            float endTime = t * 6;
            Quaternion rotateTo = new Quaternion(-180, 0, 0, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, t);
            //transform.rotation = Quaternion.Lerp(originalRotation ,Quaternion(0,180,0,0),1-(time / originalTime));
            //_targetRotation = new Quaternion(currentRot.x, currentRot.y - 180, currentRot.z, currentRot.w);
            //transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, turningRate * Time.deltaTime);
            yield return null;
        }
        animating = false;
        clicked = false;
        cb();
    }

    public void triggerRecoiling(System.Action ac) {
        StartCoroutine(inverseScale(ac));
    }

    IEnumerator inverseScale(System.Action ac) {
        float scaleDuration = 1;
        Vector3 sm_actualScale = transform.localScale;
        Vector3 sm_targetScale = new Vector3(1.0f / scaleFactor, -3.0f / scaleFactor, 7.0f / scaleFactor);     // scale of the object at the end of the animation
        for (float t = 0; t < 1; t += Time.deltaTime / scaleDuration) {
            transform.localScale = Vector3.Lerp(sm_actualScale, sm_targetScale, t);
            //transform.Rotate(Vector3.up * (t * 6));
            float endTime = t * 6;
            Quaternion rotateTo = new Quaternion(180, 0, 0, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, t);
            yield return null;
        }
        ac();
    }

    void OnMouseDown() {
        if (!coolingDown) {
            touchTing.Play();
            List<GameObject> gos = new List<GameObject>();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("note")) {
                if (go.Equals(this.gameObject))
                    continue;
                gos.Add(go);
            }
            //click it, sets previous as current if prev is null
            foreach (GameObject go in gos) {
                go.GetComponent<MessageScript>().coolingDown = true;
            }
            lastNoteRefScript.setPrevious(this.gameObject);
            animating = true;
            clicked = true;
            coolingDown = true;
            StartCoroutine(ScaleObject(() => {
                foreach (GameObject go in gos) {
                    go.GetComponent<MessageScript>().coolingDown = false;
                    firebaseScript.setNoteToRead(index);
                }
            }));
        }
    }
}
