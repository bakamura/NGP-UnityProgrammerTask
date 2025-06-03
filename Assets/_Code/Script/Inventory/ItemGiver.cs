using UnityEngine;
using NGPTask.Player;

namespace NGPTask.Object {
    public class ItemGiver : MonoBehaviour {

        [SerializeField] private Item _itemType;
        [SerializeField, Min(1)] private int _amountGiven;

        public void TryGive() {
            if (_itemType != null) _amountGiven = Inventory.Instance.TryAdd(_itemType, _amountGiven);
            else Debug.LogWarning($"No Item Type assigned to {nameof(ItemGiver)} '{name}', couldn't give");
        }

    }
}
