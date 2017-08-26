using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour {

    private const string ICONS_PATH = "Icons/";
    /// UI Strings
    private const string EQUIPPED_ITEM_LABEL = "Equipped";
    /// Formatters
    private const string ITEM_COUNT_LABEL = "X{0}";
    private const string ITEM_ACTION_LABEL = "{0}:";
    /// Items
    private const string HEALTH_ITEM = "health";

    [SerializeField] private Image[] ItemIcons;
    [SerializeField] private Text[] ItemLabels;

    [SerializeField] private Text CurItemlabel;
    [SerializeField] private Button EquipButton;
    [SerializeField] private Button UseButton;

    public int health = 25;

    private string CurItem;

    public void Refresh() {
        List<string> itemList = Managers.Inventory.GetItemList();
        int length = ItemIcons.Length;
        
        /// Add icon into available slot
        for (int i = 0; i < length; i++) {
            if (i < itemList.Count) {
                ItemIcons[i].gameObject.SetActive(true);
                ItemLabels[i].gameObject.SetActive(true);

                string item = itemList[i];

                /// Set sprite into icon and resize to native size
                Sprite sprite = Resources.Load<Sprite>(ICONS_PATH + item);
                ItemIcons[i].sprite = sprite;
                ItemIcons[i].SetNativeSize();

                /// Set item count labels
                int itemCount = Managers.Inventory.GetItemCount(item);
                string message = String.Format(ITEM_COUNT_LABEL, itemCount);

                if (item == Managers.Inventory.EquippedItem) {
                    message = EQUIPPED_ITEM_LABEL + "\n" + message;
                }

                ItemLabels[i].text = message;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((BaseEventData data) => {
                    OnItem(item);
                });

                EventTrigger trigger = ItemIcons[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear();
                trigger.triggers.Add(entry);
            } else {
                ItemIcons[i].gameObject.SetActive(false);
                ItemLabels[i].gameObject.SetActive(false);
            }

            if (!itemList.Contains(CurItem)) {
                CurItem = null;
            }

            /// Hide buttons if no item selected
            if (CurItem == null) {
                CurItemlabel.gameObject.SetActive(false);
                EquipButton.gameObject.SetActive(false);
                UseButton.gameObject.SetActive(false);
            } else {
                CurItemlabel.gameObject.SetActive(true);
                EquipButton.gameObject.SetActive(true);
                UseButton.gameObject.SetActive(CurItem == HEALTH_ITEM);

                CurItemlabel.text = String.Format(ITEM_ACTION_LABEL, CurItem);
            }
        }
    }

    /// <summary>
    /// Called by mouse click listener
    /// </summary>
    /// <param name="item">The clicked item</param>
    public void OnItem(string item) {
        CurItem = item;
        Refresh();
    }

    public void OnEquip() {
        Managers.Inventory.EquipItem(CurItem);
        Refresh();
    }

    public void OnUse() {
        Managers.Inventory.ConsumeItem(CurItem);
        if (CurItem == HEALTH_ITEM) {
            Managers.Player.ChangeHealth(health);
        }

        Refresh();
    }
}
