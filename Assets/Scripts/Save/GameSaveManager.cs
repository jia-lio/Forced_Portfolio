using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Nullbytes
{
    public class GameSaveManager : ISubManager
    {
        public bool HasChapterSave => IsChapterEmpty(_cache.chapter) == false;
        public bool HasUserSave => IsSaveFile();
        
        private SaveContext _context;

        private readonly List<IUserSaveable> userSaves = new();
        private readonly List<IChapterSaveable> chapterSaves = new();

        private RootSave _cache = new();
        public ChapterSave ChapterData() => _cache?.chapter;
        
        private string savePath => Path.Combine(Application.persistentDataPath, "save.json");

        public int CurrentChapter => Mathf.Max(1, _cache.user.chapterID);
        
        public void Initialize(ManagerContext context)
        {
            _context = context as SaveContext;

            Directory.CreateDirectory(Path.GetDirectoryName(savePath) ?? Application.persistentDataPath);
            
            LoadRoot();
        }

        #region Register
        
        public void RegisterUser(IUserSaveable user)
        {
            if (user != null && userSaves.Contains(user) == false)
            {
                userSaves.Add(user);
            }
        }
        
        public void RegisterChapter(IChapterSaveable chapter)
        {
            if (chapter != null && chapterSaves.Contains(chapter) == false)
            {
                chapterSaves.Add(chapter);
            }
        }
        
        #endregion

        #region Save

        public void SaveUser()
        {
            var snap = new UserSave();
            foreach (var data in userSaves)
            {
                data.Save(snap);
            }

            _cache.user = snap;
            
            var json = JsonConvert.SerializeObject(_cache, SaveSettings);
            WriteJsonAtomic(savePath, json);
            
            Logger.Log(GetType(), LogType.Log, "Save - User");
        }

        public void SaveChapter()
        {
            var snap = new ChapterSave();
            foreach (var chapter in chapterSaves)
            {
                chapter.Save(snap);
            }

            _cache.chapter = snap;
            
            var json = JsonConvert.SerializeObject(_cache, SaveSettings);
            WriteJsonAtomic(savePath, json);
            
            Logger.Log(GetType(), LogType.Log, "Save - Chapter");
        }
        
        public void SaveAll()
        {
            var userSnap = new UserSave();
            foreach (var data in userSaves)
            {
                data.Save(userSnap);
            }
            
            _cache.user = userSnap;
            
            var snap = new ChapterSave();
            foreach (var chapter in chapterSaves)
            {
                chapter.Save(snap);
            }

            _cache.chapter = snap;

            var json = JsonConvert.SerializeObject(_cache, SaveSettings);
            WriteJsonAtomic(savePath, json);
            
            Logger.Log(GetType(), LogType.Log, "Save - All");
        }

        #endregion

        #region Load

        private void LoadRoot()
        {
            if (File.Exists(savePath) == false)
            {
                _cache = new();
                UserLoad();
                return;
            }

            try
            {
                var json = File.ReadAllText(savePath);
                _cache = new RootSave();
                JsonConvert.PopulateObject(json, _cache, LoadSettings);
                UserLoad();
                
                Logger.Log(GetType(), LogType.Log, "Load");
            }
            catch (Exception e)
            {
                Logger.Log(GetType(), LogType.Error, $"Load Error \n{e}");
            }
        }
        
        public void ApplyLoadSate()
        {
            UserLoad();
            ChapterLoad();
        }

        public void UserLoad()
        {
            foreach (var data in userSaves)
            {
                data.Load(_cache.user);
            }
        }

        public void ChapterLoad()
        {
            // if (HasChapterSave == false)
            //     return;
            
            foreach (var data in chapterSaves)
            {
                data.Load(_cache.chapter);
            }
        }

        #endregion

        #region Chapter Flow

        public void SetCurrentChapter(int chapterID, bool saveImmediately = true)
        {
            _cache.user.chapterID = Math.Max(1, chapterID);
            if (saveImmediately)
            {
                SaveAll();
            }
        }

        #endregion

        private static void WriteJsonAtomic(string path, string json)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(dir) == false && Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }

                var tmp = path + ".tmp";
                File.WriteAllText(tmp, json);
                
#if UNITY_EDITOR || UNITY_STANDALONE
                if (File.Exists(path))
                {
                    File.Replace(tmp, path, null);
                }
                else
                {
                    File.Move(tmp, path);
                }
#else
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                
                File.Move(tmp, path);
#endif
            }
            catch (Exception e)
            {
                Logger.Log(null, LogType.Error, $"Json Error !! \n{e}");
            }
        }

        private static bool IsChapterEmpty(ChapterSave data)
        {
            if (data == null)
                return true;

            if (data.player == null)
                return true;

            var pos = new Vector3(data.player.posX, data.player.posY, data.player.posZ);
            if (pos == default)
                return true;

            return false;
        }

        private bool IsSaveFile()
        {
            if (File.Exists(savePath) == false)
                return false;

            return true;
        }

        private static readonly JsonSerializerSettings LoadSettings = new()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        private static readonly JsonSerializerSettings SaveSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public void ResetChapterSaveData()
        {
            _cache.chapter = new();
            
            WriteJsonAtomic(savePath, JsonUtility.ToJson(_cache));
        }
        
        public void CleanUp() 
        {
            userSaves.RemoveAll(x => x == null || !(x is ISubManager));
            chapterSaves.RemoveAll(x => x == null || !(x is ISubManager));
            LoadRoot();
        }
        public void ManualUpdate() { }
        public void ManualLateUpdate() { }
        public void ManualFixedUpdate() { }
    }
}