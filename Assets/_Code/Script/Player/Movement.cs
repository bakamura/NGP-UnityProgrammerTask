using UnityEngine;
using UnityEngine.InputSystem;

namespace NGPTask.Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour {

        [Header("Inputs")]

        [SerializeField] private InputActionReference _movementActionRef;

        [Header("Atributes")]

        [SerializeField, Min(0f)] private float _speedMax;
        [Tooltip("Leave at 0 to achieve Speed Max immediately")]
        [SerializeField, Min(0f)] private float _acceleration;

        [Header("Cache")]

        private Rigidbody2D _rigidBody;

        private Vector2 _direction;
        private Vector2 _linearVelocity;

        private void Awake() {
            if (!TryGetComponent(out _rigidBody)) Debug.LogError($"No '{nameof(Rigidbody2D)}' attached to {nameof(Movement)}");
            if (_movementActionRef == null) Debug.LogError($"No asset assigned to 'Movement Action Ref'");
        }

        private void Start() {
            if (_movementActionRef?.action != null) {
                _movementActionRef.action.performed += DirectionUpdate;
                _movementActionRef.action.canceled += DirectionUpdate;
            }
        }

        private void FixedUpdate() {
            Move();
        }

        private void OnDestroy() {
            if (_movementActionRef?.action != null) {
                _movementActionRef.action.performed -= DirectionUpdate;
                _movementActionRef.action.canceled -= DirectionUpdate;
            }
        }

        private void DirectionUpdate(InputAction.CallbackContext context) {
            _direction = context.ReadValue<Vector2>();
        }

        private void Move() {
            _linearVelocity = _acceleration > 0f ?
                              Vector2.MoveTowards(_linearVelocity, _direction * _speedMax, _acceleration * Time.fixedDeltaTime) :
                              _rigidBody.linearVelocity = _direction * _speedMax;
            _rigidBody.linearVelocity = _linearVelocity;
        }

    }
}