using UnityEngine;
using NGPTask.Item;

namespace NGPTask.Debugging {
    public class UseItem : MonoBehaviour {
    
        public void UseItemAt(int slotIndex) {
            Inventory.Instance.UseItemAt(slotIndex);
        }
    
    }
}