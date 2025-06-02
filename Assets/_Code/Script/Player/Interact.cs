using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using NGPTask.Object;

namespace NGPTask.Player {
    public class Interact : Singleton<Interact> {

        [Header("Inputs")]

        [SerializeField] private InputActionReference _interactActionRef;

        [Header("Attributes")]

        [SerializeField] private Collider2D _interactCollider;

        [Header("Cache")]

        private List<Collider2D> _interactablesInRange;
        private IInteractable _interactableClosest;

        private void Start() {
            _interactActionRef.action.performed += TryInteract;

            Movement.Instance.OnMove.AddListener(InteractPointUpdate);
        }

        private void OnDestroy() {
            _interactActionRef.action.performed -= TryInteract;

            Movement.Instance.OnMove.RemoveListener(InteractPointUpdate);
        }

        private void InteractPointUpdate(Vector2 direction) {
            if (direction != Vector2.zero) _interactCollider.transform.localPosition = direction.normalized;
        }

        private void TryInteract(InputAction.CallbackContext context) {
            if (_interactCollider.Overlap(_interactablesInRange) > 0) {
                if (_interactablesInRange.OrderBy(collider => Vector2.Distance(_interactCollider.transform.position, collider.transform.position)).FirstOrDefault().TryGetComponent(out _interactableClosest))
                    _interactableClosest.Interact();
                else Debug.LogWarning($"'{_interactableClosest.name}' doesn't have {nameof(IInteractable)}, but is in the Interactable Layer!");
            }
        }

    }
}
