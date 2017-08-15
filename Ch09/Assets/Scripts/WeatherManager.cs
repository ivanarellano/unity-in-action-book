using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour, IGameManager {

    public ManagerStatus status { get; private set; }

    private NetworkService networkService;

    public void Startup(NetworkService service) {
        Debug.Log("Weather service starting ...");
        networkService = service;

        GetData();

        status = ManagerStatus.Initializing;
    }

    private void GetData() {
        StartCoroutine(networkService.GetWeatherXML(OnXMLDataLoaded));
    }

    private void OnXMLDataLoaded(string response) {
        Debug.Log(response);
        status = ManagerStatus.Started;
    }
}
