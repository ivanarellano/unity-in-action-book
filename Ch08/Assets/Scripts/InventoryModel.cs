using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : MonoBehaviour, IGameManager {

    public ManagerStatus status { get; private set; }
    public string equippedItem { get; private set; }

    private Dictionary<string, int> items;

    public void Startup() {
        Debug.Log("Inventory model starting...");

        items = new Dictionary<string, int>();

        status = ManagerStatus.Started;
    }

    private void DisplayItems() {
        string itemDisplay = "Items: ";
        foreach (KeyValuePair<string, int> item in items) {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        Debug.Log(itemDisplay);
    }

    public void AddItem(string name) {
        if (items.ContainsKey(name)) {
            items[name]++;
        } else {
            items[name] = 1;
        }

        DisplayItems();
    }

    public List<string> GetItemList() {
        return new List<string>(items.Keys);
    }

    public int GetItemCount(string name) {
        if (items.ContainsKey(name)) {
            return items[name];
        }
        return 0;
    }

    public bool EquipItem(string item) {
        if (items.ContainsKey(item) && equippedItem != item) {
            equippedItem = item;
            return true;
        }
        equippedItem = null;
        return false;
    }

    public bool ConsumeItem(string name) {
        if (items.ContainsKey(name)) {
            items[name]--;

            if (items[name] == 0) {
                items.Remove(name);
            }
        } else {
            Debug.Log("cannot consume " + name);
            return false;
        }

        DisplayItems();
        return true;
    }
}
