using UnityEngine;

namespace NGPTask {
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

        public static T Instance { get; private set; }

        protected virtual void Awake() {
            if (Instance == null) Instance = this as T;
            else if (Instance != null) {
                Debug.LogWarning($"Multiple instances of {nameof(T)}, deleting duplicate '{name}'");
                Destroy(gameObject);
            }
        }

    }
}
