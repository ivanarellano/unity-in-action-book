using System;
using System.Collections;
using UnityEngine;

public class NetworkService {

    private readonly string WEATHER_XML_API;
    private readonly string WEATHER_JSON_API;
    private const string WEB_IMAGE = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    public NetworkService(string apiKey) {
        string appId = "&appid=" + apiKey;

        WEATHER_XML_API = "http://api.openweathermap.org/data/2.5/weather?q=Philadelphia,us&mode=xml" + appId;
        WEATHER_JSON_API = "http://api.openweathermap.org/data/2.5/weather?q=Philadelphia,us" + appId;
    }

    private bool IsResponseValid(WWW www) {
        if (!string.IsNullOrEmpty(www.error)) {
            Debug.Log("error: " + www.error);
            return false;
        } else if (string.IsNullOrEmpty(www.text)) {
            Debug.Log("bad data");
            return false;
        } else {
            return true;
        }
    }

    private IEnumerator CallWeatherAPI(string url, Action<string> callback) {
        WWW response = new WWW(url);
        yield return response;

        if (!IsResponseValid(response)) {
            yield break;
        }

        callback(response.text);
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback) {
        WWW www = new WWW(WEB_IMAGE);
        yield return www;
        callback(www.texture);
    }

    public IEnumerator GetWeatherXML(Action<string> callback) {
        return CallWeatherAPI(WEATHER_XML_API, callback);
    }

    public IEnumerator GetWeatherJSON(Action<string> callback) {
        return CallWeatherAPI(WEATHER_JSON_API, callback);
    }
}
