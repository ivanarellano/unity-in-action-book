using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {

    [SerializeField] private Material sky;
    [SerializeField] private Light sun;

    private float fullIntensity;

    private void Awake() {
        Messenger.AddListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    void Start () {
        fullIntensity = sun.intensity;
	}

    private void OnDestroy() {
        Messenger.RemoveListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    private void SetOvercast(float value) {
        sky.SetFloat("_Blend", value);
        sun.intensity = fullIntensity - (fullIntensity * value);
    }

    private void OnWeatherUpdated() {
        SetOvercast(Managers.Weather.cloudValue);
    }
}
