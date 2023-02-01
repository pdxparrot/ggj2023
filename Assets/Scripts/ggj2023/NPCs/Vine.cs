using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.NPCs
{
    public sealed class Vine : NPC25D
    {
        public VineBehavior VineBehavior => (VineBehavior)NPCBehavior;

        [SerializeField]
        [ReadOnly]
        private int _health;

        public int Health => _health;

        public bool IsDead => Health <= 0;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

            SetPassive();
        }

        protected override void OnDestroy()
        {
            if(NPCManager.HasInstance) {
                NPCManager.Instance.UnregisterNPC(this);
            }

            base.OnDestroy();
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(Behavior is VineBehavior);
        }

        public void Kill()
        {
            if(IsDead) {
                return;
            }

            Debug.Log($"Killing vine {name}!");

            Damage(_health);
        }

        public void Damage(int amount)
        {
            if(IsDead) {
                return;
            }

            Debug.Log($"Vine {name} hit for {amount}");

            _health -= amount;
            if(IsDead) {
                Debug.Log($"Vine {name} is dead!");

                _health = 0;
                // TODO: animate the death and then despawn

                DeSpawn(false);

                NPCManager.Instance.Boss.Damage(NPCManager.Instance.VineData.DeathDamage);
            }

            NPCManager.Instance.Boss.Damage(amount);
        }

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            SetPassive();

            _health = NPCManager.Instance.VineData.MaxHealth;

            return true;
        }

        public override void OnDeSpawn()
        {
            base.OnDeSpawn();
        }

        #endregion
    }
}
