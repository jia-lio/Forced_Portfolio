using System;
using System.Collections.Generic;

namespace Nullbytes
{
    public interface IUserSaveable
    {
        void Save(UserSave data);
        void Load(UserSave data);
    }

    public interface IChapterSaveable
    {
        void Save(ChapterSave data);
        void Load(ChapterSave data);
    }

    #region User / Chapter

    [Serializable]
    public class RootSave
    {
        public UserSave user = new();
        public ChapterSave chapter = new();
    }
    
    [Serializable]
    public class UserSave
    {
        public int chapterID = 1;
        public SoundSave sound = new();
        public UserOptionSave option = new();
    }

    [Serializable]
    public class ChapterSave
    {
        public PlayerSave player = new();
        public InventorySave inventory = new();
        public DirectSave direct = new();
        public InteractSave interact = new();
        public SaveObjectSave so = new();
        public CutSceneSave cutscene = new();
        public TutorialSave tutorial = new();
    }

    [Serializable]
    public class UserOptionSave
    {
        public ControllerSave controller = new();
        public GraphicOptionSave graphic = new();
        public SoundOptionSave sound = new();
    }

    [Serializable]
    public class ControllerSave
    {
        public float sensitivity = 0.15f;
    }
    
    [Serializable]
    public class GraphicOptionSave
    {
        public WindowMode windowMode = WindowMode.Full;
        public float gamma = 0.2f;
        public Quality quality;
    }

    [Serializable]
    public class SoundOptionSave
    {
        public float masterSound = 1f;
        public float musicSound = 1f;
        public float sfxSound = 1f;
    }

    [Serializable]
    public class PlayerSave
    {
        public float posX;
        public float posY;
        public float posZ;
        public float rotY;
        public string amdKey;
        public string snapKey = string.Empty;
    }
    
    [Serializable]
    public class InventorySave
    {
        public List<int> inventoryItemIDs = new();
    }
    
    [Serializable]
    public class SoundSave
    {
        public string ambKey;
    }

    [Serializable]
    public class DirectSave
    {
        public Dictionary<string, bool> completeDirects = new();
    }

    [Serializable]
    public class InteractSave
    {
        public Dictionary<string, bool> completeInteract = new();
        public Dictionary<string, bool> minigameComplete = new();
        public Dictionary<string, Dictionary<int, bool>> smPuzzleComplete = new();
    }

    [Serializable]
    public class SaveObjectSave
    {
        public Dictionary<string, bool> saveActive = new();
    }

    [Serializable]
    public class CutSceneSave
    {
        public Dictionary<string, bool> cutsceneComplete = new();
    }

    [Serializable]
    public class TutorialSave
    {
        public Dictionary<string, bool> tutorialComplete = new();
    }
    #endregion

}