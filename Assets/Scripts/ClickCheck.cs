using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class goldenCombination : MonoBehaviour {
    private List<GameObject> goodArray;

    // private List

    public goldenCombination() {
        goodArray = new List<GameObject>();
    }

    public void pushNewObject(GameObject newObj, System.Action<bool, GameObject> cb) {
        Debug.Log(goodArray);
        if (goodArray.Count < 3) {
            goodArray.Add(newObj);
            //newObj.GetComponent<SpriteRenderer>().color = Color.blue;

            cb(true, newObj);
        } else {
            foreach (GameObject obj in goodArray) {
                //obj.GetComponent<SpriteRenderer>().color = Color.white;
                cb(false, obj);
            }
            goodArray = new List<GameObject>();
        }
    }

    public bool isArrayValid() {
        bool isValid = false;
        if (goodArray.Count == 3) {
            if (goodArray[0].gameObject.name == "Turtle" && goodArray[1].gameObject.name == "Love" && goodArray[2].gameObject.name == "Carpark") {
                isValid = true;
            }
        }
        return isValid;
    }
}

public class ClickCheck : MonoBehaviour {
    [SerializeField]
    private Text loadText;
    private bool didWin = false;
    private bool loadScene = false;

    GameObject previousHit;
    Vector3 previousScale = new Vector3(1.577541f, 1.577541f, 1.577541f);
    goldenCombination winConfig;
    GameObject winrarFooty;
    bool gameLoop = true;
    bool routinesRunning = true;

    void Start() {
        winrarFooty = GameObject.FindGameObjectWithTag("gen");
        winConfig = new goldenCombination();
    }

    void Awake() {
        
    }

    IEnumerator LoadNewScene() {     
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            loadText.text = (async.progress * 100).ToString();
            yield return null;
        }

    }


    void Update() {
        if (gameLoop) {
            GameObject hoveringOver = checkHover();
            if (hoveringOver) {
                growBigSmall(hoveringOver);
                checkClick(hoveringOver);
            } else if (previousHit && previousHit.transform.localScale != previousScale) {
                previousHit.transform.localScale = previousScale;
            }
        }
    }

    void growBigSmall(GameObject hoverObj) {
        float fixSin = Mathf.Sin(Time.time * 6f) * 0.02f;
        hoverObj.transform.localScale += new Vector3(fixSin, fixSin);
    }

    void checkClick(GameObject hoverObj) {
        if (Input.GetMouseButtonDown(0)) {
            print("you clicked on " + hoverObj.name.ToString());
            winConfig.pushNewObject(hoverObj, (bool state, GameObject passedObj) => {
                if (state) {
                    //passedObj.transform.Rotate(0, 0, 50f);
                    routinesRunning = true;
                    StartCoroutine(selectBob(passedObj));
                } else {
                    routinesRunning = false;
                    //passedObj.transform.Rotate(0, 0, -50f);
                    //StopCoroutine(selectBob(passedObj));
                }
            });
            if (winConfig.isArrayValid()) {
                print("YES");
                gameLoop = false;
                StartCoroutine(LoadNewScene());
                StartCoroutine(endSec());
            };
        }
    }

    IEnumerator selectBob(GameObject obj) {
        float t = 0;
        while (routinesRunning) {
            t += Time.deltaTime;
            obj.transform.Rotate(0, 0, Mathf.Sin(t * 20) * 4);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator endSec() {
        StartCoroutine(continuousRotation());
        yield return new WaitForSeconds(3f);
        didWin = true;
        //SceneManager.LoadScene(1);
    }

    IEnumerator continuousRotation() {
        while (true) {
            winrarFooty.transform.Rotate(Vector3.forward * Time.deltaTime * 200);
            winrarFooty.transform.localScale += new Vector3(4f * Time.deltaTime, 4f * Time.deltaTime, 1);
            yield return new WaitForSeconds(0.01f);
        }


    }

    GameObject checkHover() {
        GameObject hoverObj = null;
        Vector2 outRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D outHit = Physics2D.Raycast(outRay, Vector2.zero);
        if (outHit) {
            if (!previousHit) {
                previousHit = outHit.collider.gameObject;
                Debug.Log("We hit " + outHit.collider.name);
            } else {
                if (previousHit.gameObject.GetInstanceID() != outHit.collider.gameObject.GetInstanceID()) {
                    //different
                    Debug.Log("We different hit " + outHit.collider.name);

                } else {
                    print("same");
                    hoverObj = outHit.collider.gameObject;
                }
            }
            previousHit = outHit.collider.gameObject;
        }
        return hoverObj;
    }
}
