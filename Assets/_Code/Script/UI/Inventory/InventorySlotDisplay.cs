using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using NGPTask.Item;

namespace NGPTask.UI {
    public class InventorySlotDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        public ItemType ItemTypeCurrent { get; private set; }

        [Header("References")]

        [SerializeField] private Image _iconDisplay;
        [SerializeField] private TextMeshProUGUI _amountDisplay;

        public void DisplayUpdate(Inventory.InventorySlot slotDisplayed) {
            ItemTypeCurrent = slotDisplayed.ItemType;
            _iconDisplay.sprite = ItemTypeCurrent?.Icon;
            _iconDisplay.enabled = _iconDisplay.sprite != null;
            _amountDisplay.text = slotDisplayed.ItemAmount > 1 ? $"{slotDisplayed.ItemAmount}" : "";
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if(ItemTypeCurrent != null) InventoryInfoDisplay.Instance.InfoDisplay(ItemTypeCurrent.DisplayName, ItemTypeCurrent.Description);
        }

        public void OnPointerExit(PointerEventData eventData) {
            InventoryInfoDisplay.Instance.HideDisplay();
        }

    }
}