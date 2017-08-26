using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager {

    public ManagerStatus Status { get; private set; }
    public string EquippedItem { get; private set; }

    private Dictionary<string, int> Items;
    private NetworkService NetworkService;

    public void Startup(NetworkService service) {
        Debug.Log("Inventory model starting...");

        NetworkService = service;

        Items = new Dictionary<string, int>();

        Status = ManagerStatus.Started;
    }

    private void DisplayItems() {
        string itemDisplay = "Items: ";
        foreach (KeyValuePair<string, int> item in Items) {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        Debug.Log(itemDisplay);
    }

    public void AddItem(string name) {
        if (Items.ContainsKey(name)) {
            Items[name]++;
        } else {
            Items[name] = 1;
        }

        DisplayItems();
    }

    public List<string> GetItemList() {
        return new List<string>(Items.Keys);
    }

    public int GetItemCount(string name) {
        if (Items.ContainsKey(name)) {
            return Items[name];
        }
        return 0;
    }

    public bool EquipItem(string item) {
        if (Items.ContainsKey(item) && EquippedItem != item) {
            EquippedItem = item;
            return true;
        }
        EquippedItem = null;
        return false;
    }

    public bool ConsumeItem(string name) {
        if (Items.ContainsKey(name)) {
            Items[name]--;

            if (Items[name] == 0) {
                Items.Remove(name);
            }
        } else {
            Debug.Log("cannot consume " + name);
            return false;
        }

        DisplayItems();
        return true;
    }
}
