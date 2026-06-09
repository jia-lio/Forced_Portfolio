using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nullbytes
{
    public class ExcelTable
    {
        private static ExcelTable _instance;
        public static ExcelTable Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("TableData not initialized. Call TableHelper.Initialize(config) first.");
                return _instance;
            }
        }

        private GameConfig _config;

        private ExcelTable(GameConfig config)
        {
            _config = config;
        }

        public static void Initialize(GameConfig config)
        {
            if (_instance != null)
            {
                Debug.LogWarning("TableHelper already initialized.");
                return;
            }

            _instance = new ExcelTable(config);
        }

        public TSo GetTableSO<TSo, TTable>()
            where TSo : BaseScriptableObject<TTable>
            where TTable : BaseTable
        {
            var findData = _config?.TableData.Find(x => x is TSo) as TSo;
            if (findData == null)
            {
                Logger.Log(GetType(), LogType.Error, $"Cannot Find ScriptableObject of type {typeof(TSo).Name}");
                return null;
            }

            return findData;
        }

        public TTable GetTable<TSo, TTable>(int id)
            where TSo : BaseScriptableObject<TTable>
            where TTable : BaseTable
        {
            var findData = _config?.TableData.Find(x => x is TSo) as TSo;
            if (findData == null)
            {
                Logger.Log(GetType(), LogType.Error, $"Cannot Find ScriptableObject of type {typeof(TSo).Name}");
                return null;
            }

            if (findData.rows == null || findData.rows.Count == 0)
            {
                Logger.Log(GetType(), LogType.Error, $"SO of type {typeof(TSo).Name} has no rows");
                return null;
            }

            var row = findData.rows.FirstOrDefault(r => r.ID == id);
            if (row == null)
            {
                Logger.Log(GetType(), LogType.Warning, $"No entry with ID {id} in table {typeof(TSo).Name}");
            }

            return row;
        }

        public float? GetConst(string key)
        {
            var table = GetTableSO<ConstDataTableSO, ConstDataTable>();
            var data = table.rows.Find(x => x.Key == key);
            if (data == null)
            {
                Logger.Log(GetType(), LogType.Error, "Cannot find Const Data");
                return null;
            }

            return data.Value;
        }

        public SoundDataTable GetSoundData(string key)
        {
            var table = GetTableSO<SoundDataTableSO, SoundDataTable>();
            var data = table.rows.Find(x => x.Key == key);
            if (data == null)
            {
                Logger.Log(GetType(), LogType.Error, $"Cannot find Sound Data, Key Name : {key}");
                return null;
            }

            return data;
        }

        public List<TTable> GetAll<TSo, TTable>()
            where TSo : BaseScriptableObject<TTable>
            where TTable : BaseTable
        {
            var so = GetTableSO<TSo, TTable>();
            if (so == null)
            {
                Logger.Log(GetType(), LogType.Error, $"Cannot Find ScriptableObject of type {typeof(TSo).Name}");
                return null;
            }

            if (so.rows == null || so.rows.Count == 0)
            {
                Logger.Log(GetType(), LogType.Error, $"SO of type {typeof(TSo).Name} has no rows");
                return null;
            }

            return so.rows;
        }
    }
}
