using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {

    [SerializeField] private Material sky;
    [SerializeField] private Light sun;

    private float fullIntensity;
    private float cloudValue = 0.0f;

	void Start () {
        fullIntensity = sun.intensity;
	}
	
	void Update () {
        SetOvercast(cloudValue);

        if (cloudValue < 1.0f) {
            cloudValue += .005f;
        }
	}

    private void SetOvercast(float value) {
        sky.SetFloat("_Blend", value);
        sun.intensity = fullIntensity - (fullIntensity * value);
    }
}
