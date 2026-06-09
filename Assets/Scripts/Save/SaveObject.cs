using UnityEngine;

namespace Nullbytes
{
    public class SaveObject : InjectableMonoBehaviour, IChapterSaveable
    {
        [SerializeField] private string saveKey;
        
        [Inject] private GameSaveManager saveManager;

        private bool isActive = true;

        private void Start()
        {
            saveManager.RegisterChapter(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Character>();
                if (player != null)
                {
                    isActive = false;
                    gameObject.SetActive(isActive);
                    saveManager.SaveChapter();
                }
            }
        }

        private void OnLoad()
        {
            gameObject.SetActive(isActive);
        }

        public void Save(ChapterSave data)
        {
            if (data.so.saveActive == null || data.so == null)
                return;
            
            data.so.saveActive[saveKey] = isActive;
        }

        public void Load(ChapterSave data)
        {
            if (data == null || data.so == null)
                return;

            if (data.so.saveActive != null &&
                data.so.saveActive.TryGetValue(saveKey, out var value))
            {
                isActive = value;
            }

            OnLoad();
        }
    }
}