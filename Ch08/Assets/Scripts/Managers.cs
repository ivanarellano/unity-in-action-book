using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerModel))]
[RequireComponent(typeof(InventoryModel))]
public class Managers : MonoBehaviour {

    public static PlayerModel Player { get; private set; }
    public static InventoryModel Inventory { get; private set; }

    private List<IGameManager> models;

    private void Awake() {
        Player = GetComponent<PlayerModel>();
        Inventory = GetComponent<InventoryModel>();

        models = new List<IGameManager>();
        models.Add(Player);
        models.Add(Inventory);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers() {
        foreach (IGameManager model in models) {
            model.Startup();
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
}
