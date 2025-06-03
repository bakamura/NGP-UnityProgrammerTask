using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace NGPTask.UI {
    public class InventoryInfoDisplay : Singleton<InventoryInfoDisplay> {

        [Header("References")]

        [SerializeField] private TextMeshProUGUI _nameDisplay;
        [SerializeField] private TextMeshProUGUI _descriptionDisplay;

        [Header("Cache")]

        private RectTransform _rectTransform;
        [HideInInspector] public int indexCurrent;

        protected override void Awake() {
            base.Awake();

            if (!TryGetComponent(out _rectTransform)) Debug.LogError($"{nameof(InventoryInfoDisplay)} '{name}' has no {nameof(RectTransform)} attached");
            gameObject.SetActive(false);
        }

        private void Update() {
            if (gameObject.activeInHierarchy) _rectTransform.anchoredPosition = Mouse.current.position.ReadValue();
        }

        public void InfoDisplay(string name, string description) {
            gameObject.SetActive(true);
            _nameDisplay.text = name;
            _descriptionDisplay.text = description;
        }

        public void HideDisplay() {
            gameObject.SetActive(false);
            indexCurrent = -1;
        }

    }
}
