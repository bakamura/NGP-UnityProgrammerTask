using UnityEngine;
using NGPTask.Item;

namespace NGPTask.Object {
    public class ItemGiver : MonoBehaviour {

        [SerializeField] private ItemType _itemType;
        [SerializeField, Min(1)] private int _amountGiven;

        public void TryGive() {
            if (_itemType != null) _amountGiven = Inventory.Instance.TryAdd(_itemType, _amountGiven);
            else Debug.LogWarning($"No Item Type assigned to {nameof(ItemGiver)} '{name}', couldn't give");
        }

        public void TryGiveUnlimited() {
            if (_itemType != null) Inventory.Instance.TryAdd(_itemType, _amountGiven);
            else Debug.LogWarning($"No Item Type assigned to {nameof(ItemGiver)} '{name}', couldn't give");
        }

        public void TryTakeUnlimited() {
            if (_itemType != null) Inventory.Instance.TryRemove(_itemType, _amountGiven);
            else Debug.LogWarning($"No Item Type assigned to {nameof(ItemGiver)} '{name}', couldn't remove");
        }

    }
}
