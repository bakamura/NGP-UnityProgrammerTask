using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NGPTask {
    public class InputHandler : Singleton<InputHandler> {

        public enum MapNames {
            Player,
            UI,
            Dialogue
        }

        [Header("References")]

        [SerializeField] private InputActionAsset _inputActionAsset;

        protected override void Awake() {
            base.Awake();

            ToggleInputMap(MapNames.Player, false);
            ToggleInputMap(MapNames.Dialogue, false);
        }

        public void ToggleInputMap(MapNames mapName, bool isOn) => ToggleInputMap(mapName.ToString(), isOn);

        public void ToggleInputMap(string mapId, bool isOn) {
            InputActionMap map = _inputActionAsset.actionMaps.FirstOrDefault(actionMap => actionMap.name == mapId);
            if (isOn) map.Enable();
            else map.Disable();
        }

    }
}
