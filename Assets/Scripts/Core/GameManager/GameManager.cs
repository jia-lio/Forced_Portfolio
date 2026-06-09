namespace Nullbytes
{
    public class GameManager
    {
        public ManagerContainer Container => container;

        private GameConfig config;
        private ManagerContainer container;

        public bool GameOver { get; set; }

        public void Initialize(GameConfig config)
        {
            this.config = config;
            container = new ManagerContainer(this.config);

            ExcelTable.Initialize(config);

            RegisterManager<ResourceManager>();
            RegisterManager<SoundManager>();
            RegisterManager<WorldManager>();
            RegisterManager<CameraManager>();
            RegisterManager<UIManager>();
            RegisterManager<InputManager>();
            RegisterManager<GameSaveManager>();
            RegisterManager<NpcPatrolManager>();
            RegisterManager<PlayerSystemManager>();
            RegisterManager<InteractionManager>();
            RegisterManager<DirectManager>();
            RegisterManager<ItemManager>();
            RegisterManager<InventoryManager>();
            RegisterManager<UserManager>();
            RegisterManager<GammaManager>();
            RegisterManager<EffectManager>();

            ManagerContexts contexts = ManagerContextsFactory.Build(container);
            container.Resolve<ResourceManager>().Initialize(contexts.ResourceContext);
            container.Resolve<SoundManager>().Initialize(contexts.SoundContext);
            container.Resolve<WorldManager>().Initialize(contexts.WorldContext);
            container.Resolve<CameraManager>().Initialize(contexts.CameraContext);
            container.Resolve<UIManager>().Initialize(contexts.UIContext);
            container.Resolve<InputManager>().Initialize(contexts.InputContext);
            container.Resolve<NpcPatrolManager>().Initialize(contexts.PatrolContext);
            container.Resolve<PlayerSystemManager>().Initialize(contexts.PlayerSystemContext);
            container.Resolve<InteractionManager>().Initialize(contexts.InteractionContext);
            container.Resolve<DirectManager>().Initialize(contexts.DirectContext);
            container.Resolve<ItemManager>().Initialize(contexts.ItemContext);
            container.Resolve<InventoryManager>().Initialize(contexts.InventoryContext);
            container.Resolve<UserManager>().Initialize(contexts.UserContext);
            container.Resolve<GameSaveManager>().Initialize(contexts.SaveContext);
            container.Resolve<GammaManager>().Initialize(contexts.GammaContext);
            container.Resolve<EffectManager>().Initialize(contexts.EffectContext);

            Logger.Log(this.GetType(), LogType.Success, "GameManager Initialize Successfully");
        }

        public void CleanUp()
        {
            foreach (var each in container.GetAll())
            {
                each.CleanUp();
            }
            container.CleanUp();
        }

        public void ResetPool()
        {
            container.Resolve<ResourceManager>().ResetPool();
        }

        public void ManualUpdate()
        {
            foreach (var each in container.GetAll())
            {
                each?.ManualUpdate();
            }
        }

        public void ManualLateUpdate()
        {
            foreach (var each in container.GetAll())
            {
                each?.ManualLateUpdate();
            }
        }

        public void ManualFixedUpdate()
        {
            foreach (var each in container.GetAll())
            {
                each?.ManualFixedUpdate();
            }
        }

        public void SetGameOver(bool flag)
        {
            GameOver = flag;
            if (flag)
            {
                TimeControl.Pause();
            }
            else
            {
                TimeControl.Resume();
            }
        }

        private void RegisterManager<T>() where T : class, ISubManager, new()
        {
            var instance = new T();
            container.Register(instance);
        }
    }
}