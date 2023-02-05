using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2023.Data.NPCs;
using pdxpartyparrot.ggj2023.UI;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.NPCs
{
    // TODO: should this be a behavior component like the player??
    public sealed class BlackberryMonsterBehavior : NPCBehavior
    {
        private BlackberryMonster BlackberryMonster => (BlackberryMonster)Owner;

        private BlackberryMonsterBehaviorData BlackberryMonsterBehaviorData => (BlackberryMonsterBehaviorData)BehaviorData;

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

        public int MaxHealth => BlackberryMonsterBehaviorData.MaxHealth;

        private float HealthPercent => Mathf.Clamp(Health / (float)MaxHealth, 0.0f, 1.0f);

        public bool IsDead => Health <= 0;

        public override Vector3 MoveDirection => BlackberryMonster.MoveDirection;

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is BlackberryMonster);
            Assert.IsTrue(behaviorData is BlackberryMonsterBehaviorData);

            base.Initialize(behaviorData);
        }

        public void Kill()
        {
            if(IsDead) {
                return;
            }

            Debug.Log($"Killing boss {name}!");

            Damage(_health);
        }

        public void Damage(int amount)
        {
            if(IsDead) {
                return;
            }

            Debug.Log($"Boss {name} hit for {amount}");

            _health -= amount;
            if(IsDead) {
                Debug.Log($"Boss {name} is dead!");

                _health = 0;

                _deathEffect.Trigger();

                GameManager.Instance.GameOver();
            } else {
                _hitEffect.Trigger(() => OnIdle());
            }

            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateBossHealthPercent(HealthPercent);
        }

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(base.OnSpawn(spawnpoint)) {
                return true;
            }

            BlackberryMonster.SetPassive();

            _health = MaxHealth;
            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateBossHealthPercent(HealthPercent);

            return false;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(base.OnReSpawn(spawnpoint)) {
                return true;
            }

            BlackberryMonster.SetPassive();

            _health = MaxHealth;
            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdateBossHealthPercent(HealthPercent);

            return false;
        }

        #endregion
    }
}
