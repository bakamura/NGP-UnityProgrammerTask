using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using NGPTask.Item;

namespace NGPTask.UI {
    [RequireComponent(typeof(Image))]
    public class InventoryDragDisplay : Singleton<InventoryDragDisplay> {

        //[Header("Inputs")]

        //[SerializeField] private InputActionReference _releaseReference;

        [Header("Cache")]

        private Image _image;
        private RectTransform _rectTransform;
        private int _indexCurrent;

        protected override void Awake() {
            base.Awake();

            //if (_releaseReference == null) Debug.LogWarning($"No InputAction assigned to {nameof(InventoryDragDisplay)} '{name}'. Drag will fail");
            if (!TryGetComponent(out _image)) Debug.LogError($"Couldn't get {nameof(Image)} attached to {nameof(InventoryDragDisplay)} '{name}'. Display won't refresh properly");
            if (!TryGetComponent(out _rectTransform)) Debug.LogError($"{nameof(InventoryInfoDisplay)} '{name}' has no {nameof(RectTransform)} attached");
            gameObject.SetActive(false);
        }

        //private void Start() {
        //    _releaseReference.action.canceled += TrySwap;
        //}

        //private void OnDestroy() {
        //    _releaseReference.action.canceled -= TrySwap;
        //}

        private void Update() {
            if (gameObject.activeInHierarchy) {
                _rectTransform.anchoredPosition = Mouse.current.position.ReadValue();
                if (Mouse.current.leftButton.wasReleasedThisFrame) TrySwap();
            }
        }

        public void DragDisplay(Sprite icon, int index) {
            gameObject.SetActive(true);
            _image.sprite = icon;
            _indexCurrent = index;
        }

        private void TrySwap(/*InputAction.CallbackContext context*/) {
            gameObject.SetActive(false);

            if (InventoryInfoDisplay.Instance.indexCurrent >= 0) Inventory.Instance.SlotSwap(_indexCurrent, InventoryInfoDisplay.Instance.indexCurrent);
        }

    }
}
