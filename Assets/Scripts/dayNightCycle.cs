using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightCycle : MonoBehaviour {
    public Material dawnSky;
    public Material dawnSky2;
    public Material dawnSky3;
    public Material dawnSky4;

    public Material nightSky;
    public Material nightSky2;
    public Material nightSky3;
    public Material nightSky4;
    [SerializeField]
    private TextMesh title;
    // Use this for initialization
	void Awake () {
        int currentDate = System.DateTime.Now.Day;
        print("date is " + currentDate);
        int currentHour = System.DateTime.Now.Hour;
        print(currentHour);
        if (currentHour < 6 || currentHour > 20) {
            print("its night");
            if (title != null)
                title.color = Color.white;
            if (currentDate % 5 == 0) {
                RenderSettings.skybox = nightSky2;
            } else if (currentDate % 4 == 0) {
                RenderSettings.skybox = nightSky3;
            } else if (currentDate % 3 == 0) {
                RenderSettings.skybox = nightSky4;
            } else {
                RenderSettings.skybox = nightSky;
            }
        } else if (currentDate % 5 == 0) {
            RenderSettings.skybox = dawnSky2;
        } else if (currentDate % 4 == 0) {
            RenderSettings.skybox = dawnSky3;
        } else if (currentDate % 3 == 0) {
            RenderSettings.skybox = dawnSky4;
        } else {
            RenderSettings.skybox = dawnSky;
        }
    }
}
