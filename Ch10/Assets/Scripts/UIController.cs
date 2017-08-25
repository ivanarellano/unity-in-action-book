using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private SettingsPopup popup;

	void Start () {
        popup.gameObject.SetActive(false);
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.M)) {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);

            /// Toggle cursor along with popup
            Cursor.lockState = isShowing ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isShowing;
        }
	}
}
