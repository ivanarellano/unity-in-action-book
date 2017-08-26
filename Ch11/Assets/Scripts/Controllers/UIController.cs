using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField] private Text HealthLabel;
    [SerializeField] private InventoryPopup Popup;

    private void Awake() {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

    void Start () {
        OnHealthUpdated();

        Popup.gameObject.SetActive(false);
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.M)) {
            bool isShowing = Popup.gameObject.activeSelf;
            Popup.gameObject.SetActive(!isShowing);
            Popup.Refresh();
        }
	}

    private void OnDestroy() {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }

    private void OnHealthUpdated() {
        string message = "Health: " + Managers.Player.Health + "/" + Managers.Player.MaxHealth;
        HealthLabel.text = message;
    }
}
