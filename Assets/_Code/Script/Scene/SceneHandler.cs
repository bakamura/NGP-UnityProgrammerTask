using NGPTask.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NGPTask.Scene {
    public class SceneHandler : Singleton<SceneHandler> {

        [Header("Attributes")]

        [SerializeField] private string _firstSceneLoaded;

        [Header("Cache")]

        private HashSet<string> _scenesLoaded = new HashSet<string>();

        private void Start() {
            TryLoad(_firstSceneLoaded);
        }

        public void TryLoad(string sceneName, UnityEvent onSceneLoad = null) {
            if (_scenesLoaded.Add(sceneName)) StartCoroutine(Load(sceneName, onSceneLoad));
            else Debug.LogWarning($"Can't load scene '{sceneName}': Already Loaded");
        }

        private IEnumerator Load(string sceneName, UnityEvent onSceneLoad = null) {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            onSceneLoad?.Invoke();
        }

        public void TryUnload(string sceneName) {
            if (_scenesLoaded.Remove(sceneName)) SceneManager.UnloadSceneAsync(sceneName);
            else Debug.LogWarning($"Can't unload scene '{sceneName}': Not Loaded");
        }

        public void UnloadThenLoad(string sceneUnloaded, string sceneLoaded) {
            StartCoroutine(UnloadThenLoadRoutine(sceneUnloaded, sceneLoaded));
        }

        private IEnumerator UnloadThenLoadRoutine(string sceneUnloaded, string sceneLoaded) {
            bool fadeEnd = false;
            FadeEventDisplay.Instance.FadeEvent(() => fadeEnd = true);

            yield return new WaitWhile(() => !fadeEnd);

            if (_scenesLoaded.Remove(sceneUnloaded)) yield return SceneManager.UnloadSceneAsync(sceneUnloaded);
            else Debug.LogWarning($"Can't load scene '{sceneUnloaded}': Not Loaded");

            if (_scenesLoaded.Add(sceneLoaded)) yield return SceneManager.LoadSceneAsync(sceneLoaded, LoadSceneMode.Additive);
            else Debug.LogWarning($"Can't load scene '{sceneLoaded}': Already Loaded");

            FadeEventDisplay.Instance.FadeOut();
        }

    }
}
