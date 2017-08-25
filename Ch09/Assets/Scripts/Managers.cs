using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(WeatherManager))]
[RequireComponent(typeof(ImagesManager))]
public class Managers : MonoBehaviour {

    public static WeatherManager Weather { get; private set; }
    public static ImagesManager Image { get; private set; }

    private const string KEYS_PATH = "keys.json";
    private List<IGameManager> managers;

	void Awake () {
        Weather = GetComponent<WeatherManager>();
        Image = GetComponent<ImagesManager>();

        managers = new List<IGameManager> { Weather, Image };

        StartCoroutine(StartupManagers());
    }

    [Serializable]
    public class APIKeys {
        public string openWeatherMap;
    }
	
	private IEnumerator StartupManagers () {
        if (!System.IO.File.Exists(KEYS_PATH)) {
            Debug.Log("File does not exist: " + KEYS_PATH);
            yield break;
        }

        APIKeys keys = JsonUtility.FromJson<APIKeys>(
            System.IO.File.ReadAllText(KEYS_PATH));

        NetworkService networkService = new NetworkService(keys.openWeatherMap);

        foreach (IGameManager manager in managers) {
            manager.Startup(networkService);
        }

        yield return null;

        int numModules = managers.Count;
        int numReady = 0;

        while (numReady < numModules) {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in managers) {
                if (manager.status == ManagerStatus.Started) {
                    numReady++;
                }
            }

            if (numReady > lastReady) {
                Debug.Log("Progress: " + numReady + "/" + numModules);
            }

            yield return null;
        }

        Debug.Log("All managers started up");
	}
}
