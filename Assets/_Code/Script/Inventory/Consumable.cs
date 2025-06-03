using UnityEngine;

namespace NGPTask.Item {
    public abstract class Consumable : ItemType, IUsable {

        [Header("Consumable")]

        [SerializeField, Min(1)] private int _amountToConsume;

        public bool CanUse => Inventory.Instance.GetAmountOf(this) >= _amountToConsume;

        public void Use() {
            if (CanUse) {
                Inventory.Instance.TryRemove(this, _amountToConsume);
                Effect();
            }
            else Debug.LogWarning($"Trying to use '{name}' even though Inventory doesn't have enough of it!");
        }

        public abstract void Effect();

    }
}
