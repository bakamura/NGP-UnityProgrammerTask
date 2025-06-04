using UnityEngine;

namespace NGPTask {
    public class QuitGame : MonoBehaviour {

        private void Start() {
            InputHandler.Instance.ToggleInputMap(InputHandler.MapNames.Player, false);
        }

        public void AllowPlayerInput() {
            InputHandler.Instance.ToggleInputMap(InputHandler.MapNames.Player, true);
        }

        public void Quit() => Application.Quit();

    }
}