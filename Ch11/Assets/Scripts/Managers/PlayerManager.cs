using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager {

    /// Defaults
    private const int HEALTH = 50;
    private const int MAX_HEALTH = 100;

    private NetworkService networkService;

    public ManagerStatus Status { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    public void Startup(NetworkService service) {
        Debug.Log("Player model starting...");

        Health = 50;
        MaxHealth = 100;

        Status = ManagerStatus.Started;
    }

    public void ChangeHealth(int value) {
        Health += value;

        if (Health > MaxHealth) {
            Health = MaxHealth;
        } else if (Health < 0) {
            Health = 0;
            Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }

        Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
    }

    public void UpdateData(int health, int maxHealth) {
        Health = health;
        MaxHealth = maxHealth;
    }

    public void Respawn() {
        UpdateData(HEALTH, MAX_HEALTH);
    }
}
