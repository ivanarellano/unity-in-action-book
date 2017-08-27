using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    /// UI Strings
    private const string LEVEL_COMPLETED = "Level Complete!";
    private const string LEVEL_FAILED = "Level Failed";
    private const string GAME_COMPLETED = "You Finished the Game!";
    /// Formatters
    private const string CURRENT_HEALTH = "Health: {0}/{1}";
    /// Delays
    private const int LEVEL_TRANSITION_SECONDS = 2;

    [SerializeField] private Text HealthLabel;
    [SerializeField] private Text LevelEndingLabel;
    [SerializeField] private InventoryPopup Popup;

    private void Awake() {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, OnLevelCompleted);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.AddListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    void Start () {
        OnHealthUpdated();

        LevelEndingLabel.gameObject.SetActive(false);
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
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, OnLevelCompleted);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.RemoveListener(GameEvent.GAME_COMPLETE, OnGameComplete);
    }

    private void OnHealthUpdated() {
        HealthLabel.text = String.Format(
            CURRENT_HEALTH, Managers.Player.Health, Managers.Player.MaxHealth);
    }

    private void OnLevelCompleted() {
        StartCoroutine(CompleteLevel());
    }

    private void OnLevelFailed() {
        StartCoroutine(FailLevel());
    }

    private void OnGameComplete() {
        SetLevelEndingLabel(GAME_COMPLETED);
    }

    private IEnumerator CompleteLevel() {
        SetLevelEndingLabel(LEVEL_COMPLETED);

        yield return new WaitForSeconds(LEVEL_TRANSITION_SECONDS);

        Managers.Mission.GoToNext();
    }

    private IEnumerator FailLevel() {
        SetLevelEndingLabel(LEVEL_FAILED);

        yield return new WaitForSeconds(LEVEL_TRANSITION_SECONDS);

        Managers.Mission.GoToNext();
    }

    private void SetLevelEndingLabel(string message) {
        LevelEndingLabel.gameObject.SetActive(true);
        LevelEndingLabel.text = message;
    }

    public void SaveGame() {
        Managers.Data.SaveGameState();
    }

    public void LoadGame() {
        Managers.Data.LoadGameState();
    }
}
