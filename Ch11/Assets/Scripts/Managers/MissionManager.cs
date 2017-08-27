using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager {

    /// Formatters
    private const string LEVEL_FORMAT = "Level{0}";
    /// Defaults
    private const int START_LEVEL = 0;
    private const int MAX_LEVEL = 3;

    public ManagerStatus Status { get; private set; }
    public int CurLevel { get; private set; }
    public int MaxLevel { get; private set; }

    private NetworkService NetworkService;

    public void Startup(NetworkService service) {
        Debug.Log("Mission manager starting...");

        NetworkService = service;

        UpdateData(START_LEVEL, MAX_LEVEL);

        Status = ManagerStatus.Started;
    }

    public void GoToNext() {
        if (CurLevel < MaxLevel) {
            CurLevel++;

            RestartLevel();
        } else {
            Debug.Log("Last level");
            Messenger.Broadcast(GameEvent.GAME_COMPLETE);
        }
    }

    public void ReachObjective() {
        /// Could have logic to handle multiple objectives
        Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }

    public void RestartLevel() {
        string name = String.Format(LEVEL_FORMAT, CurLevel);
        Debug.Log("Loading " + name);
        SceneManager.LoadScene(name);
    }

    public void UpdateData(int curLevel, int maxLevel) {
        CurLevel = CurLevel;
        MaxLevel = maxLevel;
    }
}
