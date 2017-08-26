using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour {

    [SerializeField] private GameObject[] targets;

    public bool requireKey;

    private void OnTriggerEnter(Collider other) {
        if (requireKey && Managers.Inventory.EquippedItem != "key") {
            return;
        }

        foreach (GameObject obj in targets) {
            obj.SendMessage("Activate");
        }
    }

    private void OnTriggerExit(Collider other) {
        foreach (GameObject obj in targets) {
            obj.SendMessage("Deactivate");
        }
    }
}
