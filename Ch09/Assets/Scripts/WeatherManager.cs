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

        status = ManagerStatus.Started;
    }
}
