using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(MissionManager))]
[RequireComponent(typeof(DataManager))]
public class Managers : MonoBehaviour {

    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static MissionManager Mission { get; private set; }
    public static DataManager Data { get; private set; }

    private const string KEYS_PATH = "keys.json";

    private List<IGameManager> ManagerList;

    [Serializable]
    public class APIKeys {
        public string openWeatherMap;
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject); /// Persists object between scenes

        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        Mission = GetComponent<MissionManager>();
        Data = GetComponent<DataManager>();

        ManagerList = new List<IGameManager>
        {
            Player, Inventory, Mission, Data
        };

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers() {
        APIKeys keys = GetAPIKeys();
        if (keys == null) yield break;

        NetworkService network = new NetworkService(keys.openWeatherMap);

        foreach (IGameManager model in ManagerList) {
            model.Startup(network);
        }

        yield return null;

        int numModules = ManagerList.Count;
        int numReady = 0;

        while (numReady < numModules) {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager model in ManagerList) {
                if (model.Status == ManagerStatus.Started) {
                    numReady++;
                }
            }

            if (numReady > lastReady) {
                Debug.Log("Progress: " + numReady + "/" + numModules);
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);
            }

            yield return null;
        }

        Debug.Log("All managers started up");
        Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
    }

    private APIKeys GetAPIKeys() {
        if (!System.IO.File.Exists(KEYS_PATH)){
            Debug.Log("File does not exist: " + KEYS_PATH);
            return null;
        }

        return JsonUtility.FromJson<APIKeys>(System.IO.File.ReadAllText(KEYS_PATH));
    }
}
