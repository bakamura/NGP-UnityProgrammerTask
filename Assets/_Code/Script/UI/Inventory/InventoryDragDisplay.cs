using NGPTask.Item;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NGPTask.UI {
    [RequireComponent(typeof(Image))]
    public class InventoryDragDisplay : Singleton<InventoryDragDisplay> {

        [Header("Inputs")]

        [SerializeField] private InputActionReference _releaseReference;

        [Header("Cache")]

        private Image _image;
        private RectTransform _rectTransform;
        private int _indexCurrent;

        protected override void Awake() {
            base.Awake();

            if (_releaseReference == null) Debug.LogWarning($"No InputAction assigned to {nameof(InventoryDragDisplay)} '{name}'. Drag will fail");
            if (!TryGetComponent(out _image)) Debug.LogError($"Couldn't get {nameof(Image)} attached to {nameof(InventoryDragDisplay)} '{name}'. Display won't refresh properly");
            if (!TryGetComponent(out _rectTransform)) Debug.LogError($"{nameof(InventoryInfoDisplay)} '{name}' has no {nameof(RectTransform)} attached");
            gameObject.SetActive(false);
        }

        private void Update() {
            if (gameObject.activeInHierarchy) _rectTransform.anchoredPosition = Mouse.current.position.ReadValue();
        }

        public void DragDisplay(Sprite icon, int index) {
            _releaseReference.action.canceled += TrySwap;
            gameObject.SetActive(true);
            _image.sprite = icon;
            _indexCurrent = index;
        }

        private void TrySwap(InputAction.CallbackContext context) {
            _releaseReference.action.canceled += TrySwap;
            gameObject.SetActive(false);

            Debug.Log(_indexCurrent);
            Debug.Log(InventoryInfoDisplay.Instance.IndexCurrent);
            if(InventoryInfoDisplay.Instance.IndexCurrent >= 0) Inventory.Instance.SlotSwap(_indexCurrent, InventoryInfoDisplay.Instance.IndexCurrent);
        }

    }
}
