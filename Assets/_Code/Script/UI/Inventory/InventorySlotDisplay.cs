using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NGPTask.Item;

namespace NGPTask.UI {
    public class InventorySlotDisplay : MonoBehaviour {

        public ItemType ItemTypeCurrent { get; private set; }

        [Header("References")]

        [SerializeField] private Image _iconDisplay;
        [SerializeField] private TextMeshProUGUI _amountDisplay;

        public void DisplayUpdate(Inventory.InventorySlot slotDisplayed) {
            ItemTypeCurrent = slotDisplayed.ItemType;
            _iconDisplay.sprite = ItemTypeCurrent.Icon;
            _amountDisplay.text = slotDisplayed.ItemAmount > 1 ? $"{slotDisplayed.ItemAmount}" : "";
        }

    }
}