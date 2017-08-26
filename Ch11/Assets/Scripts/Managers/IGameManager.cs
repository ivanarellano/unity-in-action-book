public interface IGameManager {
    ManagerStatus Status { get; }

    void Startup(NetworkService service);
}
