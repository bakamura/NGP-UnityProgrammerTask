using System.Collections.Generic;
using UnityEngine;

namespace NGPTask.Item {
    public class Inventory : Singleton<Inventory> {

        [Header("Attributes")]

        [SerializeField, Min(1)] private int _slotAmount;

        [Header("Cache")]

        private List<InventorySlot> _slots = new List<InventorySlot>();

        public IReadOnlyList<InventorySlot> Slots => _slots;

        [System.Serializable]
        public class InventorySlot {
            public ItemType ItemType { get; private set; }
            public int ItemAmount { get; private set; }

            public bool IsEmpty => ItemType == null || ItemAmount < 1;

            /// <returns>The excess amount that didn't fit.</returns>
            public int Set(ItemType type, int amount) {
                if (IsEmpty) {
                    ItemType = type;
                    return TryAdd(amount);
                }
                else Debug.LogError($"Trying to Set non-empty {nameof(InventorySlot)}");
                return amount;
            }

            /// <returns>The excess amount that didn't fit.</returns>
            public int TryAdd(int amount) {
                ItemAmount += amount;
                if (ItemAmount <= ItemType.StackSize) return 0;
                int excess = ItemAmount - ItemType.StackSize;
                ItemAmount = ItemType.StackSize;
                return excess;
            }

            /// <returns>The excess amount that couldn't be removed.</returns>
            public int TryRemove(int amount) {
                ItemAmount -= amount;
                int excess = Mathf.Abs(ItemAmount);
                if (ItemAmount <= 0) {
                    ItemType = null;
                    ItemAmount = 0;
                }
                return excess;
            }

            public void Clear() {
                ItemType = null;
                ItemAmount = 0;
            }
        }

        protected override void Awake() {
            base.Awake();

            for (int i = 0; i < _slotAmount; i++) _slots.Add(new InventorySlot());
        }

        public int GetAmountOf(ItemType type) {
            int amount = 0;
            foreach (InventorySlot slot in _slots) if (slot.ItemType == type) amount += slot.ItemAmount;
            return amount;
        }

        /// <returns>The excess amount that didn't fit.</returns>
        public int TryAdd(ItemType type, int amount = 1) {
            foreach (InventorySlot slot in _slots) {
                if (slot.ItemType == type) amount = slot.TryAdd(amount);
                if (amount <= 0) return 0;
            }
            foreach (InventorySlot slot in _slots) {
                if (slot.IsEmpty) amount = slot.Set(type, amount);
                if (amount <= 0) return 0;
            }
            Debug.LogWarning($"Couldn't add {amount} of {type.name} to inventory, some system is probably faulty");
            return amount;
        }

        /// <returns>The excess amount that couldn't be removed.</returns>
        public int TryRemove(ItemType type, int amount = 1) {
            foreach (InventorySlot slot in _slots) {
                if (slot.ItemType == type) amount = slot.TryRemove(amount);
                if (amount <= 0) return 0;
            }
            Debug.LogWarning($"Couldn't remove {amount} of {type.name} from inventory, some system is probably faulty");
            return amount;
        }

        public void SlotSwap(int indexFrom, int indexTo) {
            if (indexFrom == indexTo || indexFrom >= _slots.Count || indexTo >= _slots.Count) return;
            InventorySlot temp = _slots[indexFrom];
            _slots[indexFrom] = _slots[indexTo];
            _slots[indexTo] = temp;
        }

        public void UseItemAt(int slotIndex) {
            if (slotIndex < 0 || slotIndex >= _slots.Count || _slots[slotIndex].IsEmpty) {
                Debug.LogWarning($"Trying to Use invalid InventorySlot!");
                return;
            }
            if (_slots[slotIndex].ItemType is IUsable) {
                IUsable usableItem = _slots[slotIndex].ItemType as IUsable;
                if (usableItem.CanUse) usableItem.Use();
                else Debug.LogWarning($"Trying to Use '{_slots[slotIndex].ItemType.name}' but can't Use!");
            }
            else Debug.LogWarning($"Trying to Use '{_slots[slotIndex].ItemType.name}' but isn't IUsable!");
        }

    }
}
