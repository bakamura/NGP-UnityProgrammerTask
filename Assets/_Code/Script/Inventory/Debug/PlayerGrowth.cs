using System.Collections;
using UnityEngine;
using NGPTask.Item;

namespace NGPTask.Debugging {
    [CreateAssetMenu(fileName = "Item", menuName = "Items/ConsumableGrowth")]
    public class PlayerGrowth : Consumable {

        [Header("Player Growth [DEBUG CONSUMABLE]")]

        [SerializeField, Min(0.01f)] private float _scaleNew;
        [SerializeField, Min(0.01f)] private float _duration;

        public override void Effect() {
            Player.Movement.Instance.StartCoroutine(EffectRoutine());
        }

        private IEnumerator EffectRoutine() {
            Player.Movement.Instance.transform.localScale = _scaleNew * Vector3.one;
            
            yield return new WaitForSeconds(_duration);

            Player.Movement.Instance.transform.localScale = Vector3.one;
        }

    }
}