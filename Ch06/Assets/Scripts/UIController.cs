using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField] private Text scoreLabel;
    [SerializeField] private SettingsPopup settingsPopup;

    private int score;

    void Awake() {
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    void Start () {
        score = 0;
        scoreLabel.text = score.ToString();

        settingsPopup.Close();
	}

    void Destroy() {
        Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    private void OnEnemyHit() {
        score++;
        scoreLabel.text = score.ToString();
    }

    public void OnOpenSettings() {
        settingsPopup.Open();
    }

    public void OnPointerDown() {
        Debug.Log("OnPointerDown");
    }
}
