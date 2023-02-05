using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2023.Data.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.NPCs
{
    // TODO: should this be a behavior component like the player??
    public sealed class VineBehavior : NPCBehavior
    {
        private Vine Vine => (Vine)Owner;

        private VineBehaviorData VineBehaviorData => (VineBehaviorData)BehaviorData;

        #region Effects

        [SerializeField]
        private EffectTrigger _hitEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        #endregion

        [SerializeField]
        [ReadOnly]
        private int _health;

        public int Health => _health;

        public int MaxHealth => VineBehaviorData.MaxHealth;

        private float HealthPercent => Mathf.Clamp(Health / (float)MaxHealth, 0.0f, 1.0f);

        public bool IsDead => Health <= 0;

        public override Vector3 MoveDirection => Vine.MoveDirection;

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Vine);
            Assert.IsTrue(behaviorData is VineBehaviorData);

            base.Initialize(behaviorData);
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

                _deathEffect.Trigger(() => {
                    // vines hit for extra damage when they die
                    GameManager.Instance.CurrentLevel.Boss.BlackberryMonsterBehavior.Damage(VineBehaviorData.DeathDamage);
                });

                GameManager.Instance.CurrentLevel.UnRegisterVine(Vine);
            } else {
                _hitEffect.Trigger(() => OnIdle());
            }

            GameManager.Instance.CurrentLevel.Boss.BlackberryMonsterBehavior.Damage(amount);
        }

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(base.OnSpawn(spawnpoint)) {
                return true;
            }

            Vine.SetPassive();

            _health = MaxHealth;

            return false;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(base.OnReSpawn(spawnpoint)) {
                return true;
            }

            Vine.SetPassive();

            _health = MaxHealth;

            return false;
        }

        #endregion
    }
}
