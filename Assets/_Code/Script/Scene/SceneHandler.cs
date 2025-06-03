using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NGPTask.Scene {
    public class SceneHandler : Singleton<SceneHandler> {

        [Header("Attributes")]

        [SerializeField] private string _firstSceneLoaded;

        [Header("Cache")]

        private HashSet<string> _scenesLoaded = new HashSet<string>();

        private void Start() {
            Load(_firstSceneLoaded);
        }

        public void Load(string sceneName) {
            if (_scenesLoaded.Add(sceneName)) SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            else Debug.LogWarning($"Can't load scene '{sceneName}': Already Loaded");
        }

        public void Unload(string sceneName) {
            if (_scenesLoaded.Remove(sceneName)) SceneManager.UnloadSceneAsync(sceneName);
            else Debug.LogWarning($"Can't unload scene '{sceneName}': Not Loaded");
        }

    }
}
