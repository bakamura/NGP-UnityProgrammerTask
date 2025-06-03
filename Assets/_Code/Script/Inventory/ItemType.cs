using UnityEngine;

namespace NGPTask.Item {
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Simple")]
    public class ItemType : ScriptableObject {

        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField, Min(1)] public int StackSize { get; private set; }

    }
}
