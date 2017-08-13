using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUI : MonoBehaviour {

    private void OnGUI() {
        int posX = 10;
        int posY = 10;
        int width = 100;
        int height = 30;
        int buffer = 10;

        List<string> items = Managers.Inventory.GetItemList();
        if (items.Count == 0) {
            GUI.Box(new Rect(posX, posY, width, height), "No items");
        }
        
        foreach (string item in items) {
            int count = Managers.Inventory.GetItemCount(item);
            Texture2D image = Resources.Load<Texture2D>(item);
            GUI.Box(new Rect(posX, posY, width, height),
                    new GUIContent("{" + count + ")", image));
            posX += width + buffer;
        }

        string equipped = Managers.Inventory.equippedItem;
        if (equipped != null) {
            posX = Screen.width - (width + buffer);
            Texture2D image = Resources.Load<Texture2D>(equipped);
            GUI.Box(new Rect(posX, posY, width, height),
                    new GUIContent("Equipped", image));
        }

        posX = 10;
        posY += height + buffer;

        foreach (string item in items) {
            if (GUI.Button(new Rect(posX, posY, width, height), "Equip " + item)) {
                Debug.Log("Equiping: " + item);
                Managers.Inventory.EquipItem(item);
            }

            if (item == "health") {
                if (GUI.Button(new Rect(posX, posY + height+buffer, width, height), "Use Health")) {
                    Managers.Inventory.ConsumeItem("health");
                    Managers.Player.ChangeHealth(25);
                }
            }

            posX += width + buffer;
        }
    }
}
