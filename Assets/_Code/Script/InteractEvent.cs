using UnityEngine;
using UnityEngine.Events;

namespace NGPTask.Object {
    public class InteractEvent : MonoBehaviour, IInteractable {

        public UnityEvent OnInteract { get; private set; } = new UnityEvent();

        public void Interact() {
            OnInteract?.Invoke();
        }

    }
}
