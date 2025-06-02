using UnityEngine;

namespace NGPTask.Player {
    [RequireComponent(typeof(Animator))]
    public class Animation : MonoBehaviour {

        [Header("Cache")]

        private Animator _animator;

        private int DIRECTION = Animator.StringToHash("Direction");

        private void Awake() {
            if (!TryGetComponent(out _animator)) Debug.LogError($"No '{nameof(Animator)}' attached to {nameof(Animation)}");
            else _animator.SetFloat(DIRECTION, 2);
        }

        private void Start() {
            Movement.Instance.OnMove.AddListener(DirectionUpdate);
        }

        private void OnDestroy() {
            Movement.Instance.OnMove.RemoveListener(DirectionUpdate);
        }

        private void DirectionUpdate(Vector2 direction) {
            if (direction != Vector2.zero) _animator.SetFloat(DIRECTION, Vector2ToCardinalInt(direction));
        }

        private int Vector2ToCardinalInt(Vector2 v2) {
            return Mathf.Abs(v2.x) > Mathf.Abs(v2.y) 
                 ? (v2.x > 0 ? 1 : 3)
                 : (v2.y > 0 ? 0 : 2);
        }

    }
}
