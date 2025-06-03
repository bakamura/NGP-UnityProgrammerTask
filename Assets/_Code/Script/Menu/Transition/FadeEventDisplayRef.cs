using UnityEngine;
using UnityEngine.Events;

namespace NGPTask.UI {
    public class FadeEventDisplayRef : MonoBehaviour {

        [SerializeField] private UnityEvent _onFadeInEnd;

        public void FadeEvent() => FadeEventDisplay.Instance.FadeEvent(() => _onFadeInEnd.Invoke());

    }
}
