using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeatherManager))]
public class Managers : MonoBehaviour {

    public static WeatherManager Weather { get; private set; }

    private List<IGameManager> managers;

	void Awake () {
        Weather = GetComponent<WeatherManager>();

        managers = new List<IGameManager>();
        managers.Add(Weather);


	}
	
	private IEnumerator StartupManagers () {
        NetworkService networkService = new NetworkService();

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
