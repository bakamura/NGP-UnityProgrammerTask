using System.Collections.Generic;
using UnityEngine;
using NGPTask.Item;

namespace NGPTask.UI {
    //[RequireComponent(typeof(Menu))]
    public class InventoryDisplay : Singleton<InventoryDisplay> {

        [Header("Attributes")]

        [SerializeField] private RectTransform _slotDisplayParent;
        [SerializeField] private InventorySlotDisplay _slotDisplayPrefab;

        [Header("Cache")]

        private bool _isOpen; // Could be moved to Menu
        private bool _inventoryDirty = true;
        private List<InventorySlotDisplay> _slotDisplays = new List<InventorySlotDisplay>();

        public bool deactivateAfterFade { get; set; }

        protected override void Awake() {
            base.Awake();

            if (TryGetComponent(out Menu menu)) {
                menu.OnOpenStart.AddListener(TryRefresh);
                menu.OnCloseStart.AddListener(SetClosed); //
            }
            else Debug.LogError($"Couldn't get {nameof(Menu)} attached to {nameof(InventoryDisplay)} '{name}'. Display won't refresh properly");
        }

        private void Start() {
            Inventory.Instance.OnChange.AddListener(SetDirty);
        }

        private void OnDestroy() {
            Inventory.Instance.OnChange.RemoveListener(SetDirty);
        }

        private void SetDirty() {
            if (_isOpen) Refresh();
            else _inventoryDirty = true;
        }

        private void TryRefresh() {
            _isOpen = true;
            if (_inventoryDirty) {
                Refresh();
                _inventoryDirty = false;
            }
        }

        private void SetClosed() => _isOpen = false; //

        private void Refresh() {
            IReadOnlyList<Inventory.InventorySlot> inventorySlots = Inventory.Instance.Slots;
            if (_slotDisplays.Count != inventorySlots.Count) RefreshSizeTo(inventorySlots.Count);
            for (int i = 0; i < _slotDisplays.Count; i++) _slotDisplays[i].DisplayUpdate(inventorySlots[i], i);
        }

        private void RefreshSizeTo(int sizeNew) {
            if (_slotDisplays.Count < sizeNew) {
                while (_slotDisplays.Count != sizeNew) _slotDisplays.Add(Instantiate(_slotDisplayPrefab, _slotDisplayParent));
            }
            else while (_slotDisplays.Count != sizeNew) _slotDisplays.RemoveAt(0);
        }

        public void ToggleDisplay(bool isOn) {
            if (isOn) transform.parent.gameObject.GetComponent<Menu>().Open();
            else transform.parent.gameObject.GetComponent<Menu>().Close();
        }

        public void DeactivateIfMainMenu() {
            if (deactivateAfterFade) {
                deactivateAfterFade = false;
                ToggleDisplay(false);
            }
        }

        public void TogglePlayerMap(bool isOn) => InputHandler.Instance.ToggleInputMap(InputHandler.MapNames.Player, isOn);

#if UNITY_EDITOR
        private void OnValidate() {
            if (!TryGetComponent(out Menu menu)) Debug.LogWarning($"Please Attach a {nameof(Menu)} script to {nameof(InventoryDisplay)} '{name}'");
        }
#endif

    }
}
