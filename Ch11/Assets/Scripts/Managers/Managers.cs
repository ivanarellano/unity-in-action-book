using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
public class Managers : MonoBehaviour {

    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }

    private const string KEYS_PATH = "keys.json";

    private List<IGameManager> models;

    [Serializable]
    public class APIKeys {
        public string openWeatherMap;
    }

    private void Awake() {
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();

        models = new List<IGameManager>();
        models.Add(Player);
        models.Add(Inventory);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers() {
        APIKeys keys = GetAPIKeys();
        if (keys == null) yield break;

        NetworkService network = new NetworkService(keys.openWeatherMap);

        foreach (IGameManager model in models) {
            model.Startup(network);
        }

        yield return null;

        int numModules = models.Count;
        int numReady = 0;

        while (numReady < numModules) {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager model in models) {
                if (model.status == ManagerStatus.Started) {
                    numReady++;
                }
            }

            if (numReady > lastReady) {
                Debug.Log("Progress: " + numReady + "/" + numModules);

                yield return null;
            }
        }

        Debug.Log("All managers started up");
    }

    private APIKeys GetAPIKeys() {
        if (!System.IO.File.Exists(KEYS_PATH)){
            Debug.Log("File does not exist: " + KEYS_PATH);
            return null;
        }

        return JsonUtility.FromJson<APIKeys>(System.IO.File.ReadAllText(KEYS_PATH));
    }
}
