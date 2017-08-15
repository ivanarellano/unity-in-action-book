using System;
using System.Collections.Generic;
using System.Xml;
using MiniJSON;
using UnityEngine;

public class WeatherManager : MonoBehaviour, IGameManager {

    private NetworkService networkService;

    public ManagerStatus status { get; private set; }
    public float cloudValue { get; private set; }

    public void Startup(NetworkService service) {
        Debug.Log("Weather service starting ...");
        networkService = service;

        GetData();

        status = ManagerStatus.Initializing;
    }

    private void GetData() {
        //StartCoroutine(networkService.GetWeatherXML(OnXMLDataLoaded));
        StartCoroutine(networkService.GetWeatherJSON(OnJSONDataLoaded));
    }

    private void OnXMLDataLoaded(string response) {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(response);
        XmlNode root = doc.DocumentElement;

        XmlNode node = root.SelectSingleNode("clouds");
        string value = node.Attributes["value"].Value;

        cloudValue = Convert.ToInt32(value) / 100f;
        Debug.Log("Value: " + cloudValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }

    private void OnJSONDataLoaded(string response) {
        Dictionary<string, object> json;
        json = Json.Deserialize(response) as Dictionary<string, object>;

        Dictionary<string, object> clouds =
            (Dictionary<string, object>)json["clouds"];
        cloudValue = (long)clouds["all"] / 100f;

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }
}
