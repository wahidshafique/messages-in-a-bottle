using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBobScript : MonoBehaviour {
    Transform pos;

	void Update () {
        float randPerlin = Mathf.PerlinNoise(Time.time * 0.3f, 1);
        //print(randPerlin);
        float randSin = (Mathf.Sin(randPerlin));
        float fixSin = Mathf.Sin(Time.time) * 0.1f;
        //print(randSin);
        transform.rotation = Quaternion.Euler(0f, 0f, (randSin*2) * Mathf.Sin(Time.time * 1));
        transform.position = new Vector3(transform.position.x, fixSin + 0.65f, transform.position.z);
    }
}
