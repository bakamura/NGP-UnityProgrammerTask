using UnityEngine;

namespace NGPTask.UI {
    public class InventoryDisplayToggleRef : MonoBehaviour {

        public void ToggleDisplay(bool isOn) => InventoryDisplay.Instance.ToggleDisplay(isOn);

    }
}
