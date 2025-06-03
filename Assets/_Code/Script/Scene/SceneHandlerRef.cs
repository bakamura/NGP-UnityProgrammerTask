using UnityEngine;

namespace NGPTask.Scene {
    public class SceneHandlerRef : MonoBehaviour {

        public void Load(string sceneName) => SceneHandler.Instance.Load(sceneName);
        public void Unload(string sceneName) => SceneHandler.Instance.Unload(sceneName);

    }
}
