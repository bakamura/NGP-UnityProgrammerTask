using System;
using UnityEngine;

namespace NGPTask.UI {
    public class FadeEventDisplay : Singleton<FadeEventDisplay> {

        [Header("References")]

        [SerializeField] private Fade _fade;

        [Header("Cache")]

        private Action _fadeEventCurrent;

        protected override void Awake() {
            base.Awake();

            if (_fade) {
                _fade.OnOpenEnd.AddListener(PlayEventCurret);
            }
            else Debug.Log($"No {nameof(Fade)} assigned to {nameof(FadeEventDisplay)} '{name}'. Events won't happen");
        }

        public void FadeEvent(Action fadeEvent) {
            _fadeEventCurrent = fadeEvent;
            _fade.Open();
        }

        private void PlayEventCurret() {
            _fadeEventCurrent?.Invoke();
            _fadeEventCurrent = null;
        }

        public void FadeOut() {
            _fade.Close();
        }

    }
}
