using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using NGPTask.Item;
using UnityEngine.Events;

namespace NGPTask.Save {
    public class SaveSystem : Singleton<SaveSystem> {

        [SerializeField] private bool _loadSlot0OnSetup;
        [SerializeField] private bool _autoSaveSlot0OnQuit;

        [field: SerializeField] public UnityEvent OnLoadStart { get; private set; }
        [field: SerializeField] public UnityEvent OnLoadEnd { get; private set; }
        [field: SerializeField] public UnityEvent OnSaveStart { get; private set; }
        [field: SerializeField] public UnityEvent OnSaveEnd { get; private set; }

        private SaveProgress _progress = new SaveProgress();

        public static string ProgressFolderName { get; private set; } = "Save";
        private static string _progressPath;

        protected override void Awake() {
            base.Awake();

            _progressPath = $"{Application.persistentDataPath}/{ProgressFolderName}";
            if (!Directory.Exists(_progressPath)) Directory.CreateDirectory(_progressPath);
        }

        private void Start() {
            if (_loadSlot0OnSetup) ApplyProgressFromSlot(0);
            if(_autoSaveSlot0OnQuit) Application.quitting += SaveProgress0OnQuit;
        }

        public void SaveProgressInSlot(int saveSlotId) {
            ProgressUpdate();
            StartCoroutine(SaveProgress($"{_progressPath}/{saveSlotId}.sav"));
        }

        private IEnumerator SaveProgress(string savePath) {
            OnSaveStart.Invoke();

            Task task = File.WriteAllTextAsync(savePath, JsonUtility.ToJson(_progress));
            yield return task;

            OnSaveEnd.Invoke();
        }

        private void SaveProgress0OnQuit() {
            ProgressUpdate();
            File.WriteAllText($"{_progressPath}/{0}.sav", JsonUtility.ToJson(_progress));
        }

        private void ProgressUpdate() {
            IReadOnlyList<Inventory.InventorySlot> slots = Inventory.Instance.Slots;
            if (_progress.inventoryItems.Length != slots.Count) _progress.inventoryItems = new string[slots.Count];
            for (int i = 0; i < slots.Count; i++) _progress.inventoryItems[i] = slots[i].GetSaveString();
        }

        public void ApplyProgressFromSlot(int saveSlotId) {
            string savePath = $"{_progressPath}/{saveSlotId}.sav";
            if (File.Exists(savePath)) StartCoroutine(LoadProgress(savePath));
        }

        private IEnumerator LoadProgress(string savePath) {
            OnSaveStart.Invoke();

            Task<string> task = File.ReadAllTextAsync(savePath);
            yield return task;

            _progress = JsonUtility.FromJson<SaveProgress>(task.Result);
            Inventory.Instance.SetFromSave(_progress.inventoryItems);
            OnSaveEnd.Invoke();
        }

    }
}
