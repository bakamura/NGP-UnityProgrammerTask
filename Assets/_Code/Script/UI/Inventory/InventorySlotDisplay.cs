using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using NGPTask.Item;

namespace NGPTask.UI {
    [RequireComponent (typeof(Button))]
    public class InventorySlotDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        public ItemType ItemTypeCurrent { get; private set; }

        [Header("References")]

        [SerializeField] private Image _iconDisplay;
        [SerializeField] private TextMeshProUGUI _amountDisplay;
        private int _index;

        private void Awake() {
            if (TryGetComponent(out Button btn)) btn.onClick.AddListener(TryUse);
            else Debug.LogError($"Couldn't get {nameof(Button)} attached to {nameof(InventorySlotDisplay)} '{name}'. Use won't work properly");
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if(ItemTypeCurrent != null && !InventoryDragDisplay.Instance.gameObject.activeInHierarchy) InventoryInfoDisplay.Instance.InfoDisplay(ItemTypeCurrent.DisplayName, ItemTypeCurrent.Description);
            InventoryInfoDisplay.Instance.indexCurrent = _index;
        }

        public void OnPointerExit(PointerEventData eventData) {
            InventoryInfoDisplay.Instance.HideDisplay();
            if (ItemTypeCurrent != null && !InventoryDragDisplay.Instance.gameObject.activeInHierarchy && Mouse.current.leftButton.isPressed) InventoryDragDisplay.Instance.DragDisplay(_iconDisplay.sprite, _index);
        }

        public void DisplayUpdate(Inventory.InventorySlot slotDisplayed, int index) {
            ItemTypeCurrent = slotDisplayed.ItemType;
            _iconDisplay.sprite = ItemTypeCurrent?.Icon;
            _iconDisplay.enabled = _iconDisplay.sprite != null;
            _amountDisplay.text = slotDisplayed.ItemAmount > 1 ? $"{slotDisplayed.ItemAmount}" : "";
            _index = index;
        }

        private void TryUse() {
            if (ItemTypeCurrent != null && ItemTypeCurrent is IUsable) {
                IUsable usableItem = ItemTypeCurrent as IUsable;
                if(usableItem.CanUse) usableItem.Use();
            }
        }

    }
}