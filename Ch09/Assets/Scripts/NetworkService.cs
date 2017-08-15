using System;
using System.Collections;
using UnityEngine;

public class NetworkService : MonoBehaviour {

    private readonly string XML_API;

    public NetworkService(string apiKey) {
        XML_API = "http://api.openweathermap.org/data/2.5/weather?q=Philadelphia,us&appid=" + apiKey;
    }

    private bool IsResponseValid(WWW www) {
        if (www.error != null) {
            Debug.Log("error: " + www.error);
            return false;
        } else if (string.IsNullOrEmpty(www.text)) {
            Debug.Log("bad data");
            return false;
        } else {
            return true;
        }
    }

    private IEnumerator CallAPI(string url, Action<string> callback) {
        WWW response = new WWW(url);
        yield return null;

        if (!IsResponseValid(response)) {
            yield break;
        }

        callback(response.text);
    }

    public IEnumerator GetWeatherXML(Action<string> callback) {
        return CallAPI(XML_API, callback);
    }
}
