using System.Linq;
using UnityEngine;

namespace Nullbytes
{
    [CreateAssetMenu(menuName = "Nullbytes/EntryPoint")]
    public class EntryPoint : ScriptableObject
    {
        public static EntryPoint Instance { get; private set; }
        
        [SerializeField] 
        private GameConfig gameConfig;
        public bool OnGUI => gameConfig != null && gameConfig.OnGUI;

        public GameManager GameManager => gameManager;
        private GameManager gameManager;

        private void OnEnable()
        {
            if(Application.isPlaying == false)
                return;
            
            if(Instance != null && Instance != this)
                return;

            Instance = this;
            Initialize();
        }

        private void Initialize()
        {
            if (gameConfig == null)
            {
                Logger.Log(this.GetType(), LogType.Error, "GameConfig is not set.");
                return;
            }
            
            gameConfig.Initialize();
            
            gameManager = new GameManager();
            gameManager.Initialize(gameConfig);

            Debug.unityLogger.logHandler = new LogFilter();
            
            Localize.Initialize();
            
            Logger.Log(this.GetType(), LogType.Success, "EntryPoint Initialized successfully");
        }
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Nullbytes/EntryPoint")]
        public static void CreateAsset()
        {
            var path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save EntryPoint", "EntryPoint", "asset", "");
            if(string.IsNullOrEmpty(path))
                return;

            var instance = CreateInstance<EntryPoint>();
            UnityEditor.AssetDatabase.CreateAsset(instance, path);

            var preloaded = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            preloaded.RemoveAll(x => x is EntryPoint);
            preloaded.Add(instance);
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloaded.ToArray());
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitialize()
        {
#if UNITY_EDITOR
            var preload = UnityEditor.PlayerSettings.GetPreloadedAssets().FirstOrDefault(x => x is EntryPoint) as EntryPoint;
            preload?.OnEnable();
#endif
        }

        public void ManualUpdate() => gameManager?.ManualUpdate();
        public void ManualLateUpdate() => gameManager?.ManualLateUpdate();
        public void ManualFixedUpdate() => gameManager?.ManualFixedUpdate();
        
        [RuntimeInitializeOnLoadMethod]
        private static void CreateRunner()
        {
            var go = new GameObject("[EntryPointRunner]");
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<EntryPointRunner>();
        }
    }
}