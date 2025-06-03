using UnityEngine;
using UnityEngine.Events;

namespace NGPTask.Scene {
    public class SceneHandlerRef : MonoBehaviour {

        [SerializeField] private string _sceneName;
        [SerializeField] private UnityEvent _onSceneLoaded;

        public void Load() => SceneHandler.Instance.TryLoad(_sceneName, _onSceneLoaded);
        public void Unload() => SceneHandler.Instance.TryUnload(_sceneName);
        public void UnloadThisToLoad(string sceneLoaded) => SceneHandler.Instance.UnloadThenLoad(_sceneName, sceneLoaded);

    }
}
