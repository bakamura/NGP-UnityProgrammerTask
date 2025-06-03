using System.Collections.Generic;
using UnityEngine;
using NGPTask.Item;

namespace NGPTask.UI {
    [RequireComponent(typeof(Menu))]
    public class InventoryDisplay : MonoBehaviour {

        [Header("Attributes")]

        [SerializeField] private RectTransform _slotDisplayParent;
        [SerializeField] private InventorySlotDisplay _slotDisplayPrefab;

        [Header("Cache")]

        private bool _isOpen; // Could be moved to Menu
        private bool _inventoryDirty;
        private List<InventorySlotDisplay> _slotDisplays;

        private void Awake() {
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
            if (_inventoryDirty) {
                Refresh();
                _inventoryDirty = false;
            }
        }

        private void SetClosed() => _isOpen = false; //

        private void Refresh() {
            IReadOnlyList<Inventory.InventorySlot> inventorySlots = Inventory.Instance.Slots;
            if (_slotDisplays.Count != inventorySlots.Count) RefreshSizeTo(inventorySlots.Count);
            for (int i = 0; i < _slotDisplays.Count; i++) _slotDisplays[i].DisplayUpdate(inventorySlots[i]);
        }

        private void RefreshSizeTo(int sizeNew) {
            if (_slotDisplays.Count < sizeNew) {
                while (_slotDisplays.Count != sizeNew) _slotDisplays.Add(Instantiate(_slotDisplayPrefab, _slotDisplayParent));
            }
            else while (_slotDisplays.Count != sizeNew) _slotDisplays.RemoveAt(0);
        }

    }
}
