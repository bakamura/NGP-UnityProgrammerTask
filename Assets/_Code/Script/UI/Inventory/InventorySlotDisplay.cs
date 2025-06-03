using NGPTask.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NGPTask.UI {
    public class InventorySlotDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        public ItemType ItemTypeCurrent { get; private set; }

        [Header("References")]

        [SerializeField] private Image _iconDisplay;
        [SerializeField] private TextMeshProUGUI _amountDisplay;
        private int _index;

        public void DisplayUpdate(Inventory.InventorySlot slotDisplayed, int index) {
            ItemTypeCurrent = slotDisplayed.ItemType;
            _iconDisplay.sprite = ItemTypeCurrent?.Icon;
            _iconDisplay.enabled = _iconDisplay.sprite != null;
            _amountDisplay.text = slotDisplayed.ItemAmount > 1 ? $"{slotDisplayed.ItemAmount}" : "";
            _index = index;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if(ItemTypeCurrent != null) InventoryInfoDisplay.Instance.InfoDisplay(ItemTypeCurrent.DisplayName, ItemTypeCurrent.Description, _index);
        }

        public void OnPointerExit(PointerEventData eventData) {
            InventoryInfoDisplay.Instance.HideDisplay();
            if (ItemTypeCurrent != null && Mouse.current.leftButton.isPressed) InventoryDragDisplay.Instance.DragDisplay(_iconDisplay.sprite, _index);
        }

    }
}