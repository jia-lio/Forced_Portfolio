namespace Nullbytes
{
    public class ManagerContext
    {
        public GameConfig GameConfig { get; }

        public ManagerContext(GameConfig gameConfig)
        {
            GameConfig = gameConfig;
        }
    }
}