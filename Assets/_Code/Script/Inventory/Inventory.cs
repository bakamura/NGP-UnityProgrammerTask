using System.Collections.Generic;
using UnityEngine;

namespace NGPTask.Player {
    public class Inventory : Singleton<Inventory> {

        [Header("Attributes")]

        [SerializeField, Min(1)] private int _slotAmount;

        [Header("Cache")]

        private List<InventorySlot> _slots = new List<InventorySlot>();

        private class InventorySlot {
            public Item ItemType { get; private set; }
            public int ItemAmount { get; private set; }

            public bool IsEmpty => ItemType == null || ItemAmount < 1;

            /// <returns>The excess amount that didn't fit.</returns>
            public int Set(Item type, int amount) {
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
                int excess = ItemAmount % ItemType.StackSize;
                if (excess > 0) ItemAmount = ItemType.StackSize;
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


        /// <returns>The excess amount that didn't fit.</returns>
        public int TryAdd(Item type, int amount = 1) {
            foreach (InventorySlot slot in _slots) {
                if (slot.ItemType == type) amount = slot.TryAdd(amount);
                if (amount <= 0) return 0;
            }
            foreach (InventorySlot slot in _slots) {
                if (slot.IsEmpty) amount = slot.Set(type, amount);
                if (amount <= 0) return 0;
            }
            return amount;
        }

        /// <returns>The excess amount that couldn't be removed.</returns>
        public int TryRemove(Item type, int amount = 1) {
            foreach (InventorySlot slot in _slots) {
                if (slot.ItemType == type) amount = slot.TryRemove(amount);
                if (amount <= 0) return 0;
            }
            return amount;
        }

        public void SlotSwap(int indexFrom, int indexTo) {
            if (indexFrom == indexTo || indexFrom >= _slots.Count || indexTo >= _slots.Count) return;
            InventorySlot temp = _slots[indexFrom];
            _slots[indexFrom] = _slots[indexTo];
            _slots[indexTo] = temp;
        }

    }
}
