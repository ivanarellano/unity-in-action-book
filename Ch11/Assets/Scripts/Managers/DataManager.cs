using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour, IGameManager {

    private const string DATA_FILE = "game.dat";
    /// Data Keys
    private const string INVENTORY_KEY = "inventory";
    private const string HEALTH_KEY = "health";
    private const string MAX_HEALTH_KEY = "maxHealth";
    private const string CURRENT_LEVEL_KEY = "curLevel";
    private const string MAX_LEVEL_KEY = "maxLevel";

    public ManagerStatus Status { get; private set; }

    private NetworkService NetworkService;
    private string Filename;

    public void Startup(NetworkService service) {
        Debug.Log("Starting Data manager...");

        NetworkService = service;
        Filename = Path.Combine(Application.persistentDataPath, DATA_FILE);
        Status = ManagerStatus.Started;
    }

    public void SaveGameState() {
        Dictionary<string, object> gameState = new Dictionary<string, object>
        {
            { INVENTORY_KEY, Managers.Inventory.GetData() },
            { HEALTH_KEY,   Managers.Player.Health },
            { MAX_HEALTH_KEY, Managers.Player.MaxHealth },
            { CURRENT_LEVEL_KEY, Managers.Mission.CurLevel },
            { MAX_LEVEL_KEY, Managers.Mission.MaxLevel }
        };

        FileStream stream = File.Create(Filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gameState);
        stream.Close();
    }

    public void LoadGameState() {
        if (!File.Exists(Filename)) {
            Debug.Log("No saved game");
            return;
        }

        Dictionary<string, object> gameState;

        FileStream stream = File.Open(Filename, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        Managers.Inventory.UpdateData(gameState[INVENTORY_KEY] as Dictionary<string, int>);
        Managers.Player.UpdateData((int) gameState[HEALTH_KEY], (int) gameState[MAX_HEALTH_KEY]);
        Managers.Mission.UpdateData((int)gameState[CURRENT_LEVEL_KEY], (int)gameState[MAX_LEVEL_KEY]);
        Managers.Mission.RestartLevel();
    }
}
