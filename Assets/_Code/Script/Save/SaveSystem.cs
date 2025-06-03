using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using NGPTask.Item;
using UnityEngine.Events;

namespace NGPTask.Save {
    public class SaveSystem : Singleton<SaveSystem> {

        [field: SerializeField] public UnityEvent OnLoadStart { get; private set; }
        [field: SerializeField] public UnityEvent OnLoadEnd { get; private set; }
        [field: SerializeField] public UnityEvent OnSaveStart { get; private set; }
        [field: SerializeField] public UnityEvent OnSaveEnd { get; private set; }

        private SaveProgress _progress;

        public static string ProgressFolderName { get; private set; } = "Save";
        private static string _progressPath;

        protected override void Awake() {
            base.Awake();

            _progressPath = $"{Application.persistentDataPath}/{ProgressFolderName}";
            if (!Directory.Exists(_progressPath)) Directory.CreateDirectory(_progressPath);
        }

        public void SaveProgressInSlot(int saveSlotId) {
            ProgressUpdate();
            StartCoroutine(SaveProgress(saveSlotId));
        }

        private IEnumerator SaveProgress(int saveSlotId) {
            OnSaveStart.Invoke();

            Task task = File.WriteAllTextAsync($"{_progress}/{saveSlotId}.sav", JsonUtility.ToJson(_progress));
            yield return task;

            OnSaveEnd.Invoke();
        }

        private void ProgressUpdate() {
            IReadOnlyList<Inventory.InventorySlot> slots = Inventory.Instance.Slots;
            if (_progress.inventoryItems.Length != slots.Count) _progress.inventoryItems = new string[slots.Count];
            for (int i = 0; i < slots.Count; i++) _progress.inventoryItems[i] = slots[i].GetSaveString();
        }

        public void ApplyProgressFromSlot(int saveSlotId) {
            StartCoroutine(LoadProgress(saveSlotId));
        }

        private IEnumerator LoadProgress(int saveSlotId) {
            OnSaveStart.Invoke();

            Task<string> task = File.ReadAllTextAsync($"{_progress}/{saveSlotId}.sav");
            yield return task;

            _progress = JsonUtility.FromJson<SaveProgress>(task.Result);
            Inventory.Instance.SetFromSave(_progress.inventoryItems);
            OnSaveEnd.Invoke();
        }

    }
}
