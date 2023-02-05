using System.Collections.Generic;

using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.ggj2023.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Players
{
    [RequireComponent(typeof(Interactables3D))]
    public sealed class BeaverDamageVolume : MonoBehaviour
    {
        private Interactables _interactables;

        private HashSet<GameObject> _damaged = new HashSet<GameObject>();

        #region Unity Lifecycle

        private void Awake()
        {
            _interactables = GetComponent<Interactables>();
        }

        #endregion

        public void Damage(int amount)
        {
            foreach(var interactable in _interactables.GetInteractables<Vine>()) {
                Vine vine = interactable as Vine;
                if(_damaged.Contains(vine.gameObject)) {
                    continue;
                }

                vine.VineBehavior.Damage(amount);
                _damaged.Add(vine.gameObject);

            }

            foreach(var interactable in _interactables.GetInteractables<BlackberryMonster>()) {
                BlackberryMonster boss = interactable as BlackberryMonster;
                if(_damaged.Contains(boss.gameObject)) {
                    continue;
                }

                boss.BlackberryMonsterBehavior.Damage(amount);
                _damaged.Add(boss.gameObject);
            }
        }

        public void ResetDamaged()
        {
            _damaged.Clear();
        }

        public void StrongDamage(int amount)
        {
            foreach(var interactable in _interactables.GetInteractables<Vine>()) {
                Vine vine = interactable as Vine;
                vine.VineBehavior.Damage(amount);
            }

            foreach(var interactable in _interactables.GetInteractables<BlackberryMonster>()) {
                BlackberryMonster boss = interactable as BlackberryMonster;
                boss.BlackberryMonsterBehavior.Damage(amount);
            }
        }
    }
}
