using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.Players.BehaviorComponents;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.ggj2023.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Players
{
    // TODO: make an ActorComponent
    [RequireComponent(typeof(Interactables3D))]
    public sealed class BeaverBehavior : PlayerBehaviorComponent
    {
        #region Actions

        public class AttackAction : CharacterBehaviorAction
        {
            public static AttackAction Default = new AttackAction();
        }

        public class StrongAttackAction : CharacterBehaviorAction
        {
            public static StrongAttackAction Default = new StrongAttackAction();
        }

        #endregion

        private PlayerBehavior GamePlayerBehavior => (PlayerBehavior)PlayerBehavior;

        [SerializeField]
        private Player _owner;

        public Player Owner => _owner;

        #region Effects

        [SerializeField]
        private EffectTrigger _attackEffect;

        [SerializeField]
        private EffectTrigger _strongAttackEffect;

        [SerializeField]
        private EffectTrigger _hitEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        #endregion

        [SerializeField]
        [ReadOnly]
        private int _health;

        public int Health => _health;

        public int MaxHealth => GamePlayerBehavior.GamePlayerBehaviorData.MaxHealth;

        private float HealthPercent => Mathf.Clamp(Health / (float)MaxHealth, 0.0f, 1.0f);

        public bool IsDead => Health <= 0;

        public bool IsAttacking => _attackEffect.IsRunning;

        [SerializeField]
        [ReadOnly]
        private bool _isImmune;

        public bool IsImmune => _isImmune;

        [SerializeField]
        [ReadOnly]
        private bool _isStrongAttacking;

        public bool IsStrongAttacking => _isStrongAttacking;

        private bool CanAttack => !IsDead && !IsAttacking && !IsStrongAttacking;

        private Interactables _interactables;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            _interactables = GetComponent<Interactables>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        #endregion

        public void Kill()
        {
            if(IsDead || IsImmune || PlayerManager.Instance.PlayersImmune) {
                return;
            }

            Debug.Log($"Killing player {name}!");

            Damage(_health);
        }

        public void Damage(int amount)
        {
            if(IsDead || IsImmune || PlayerManager.Instance.PlayersImmune) {
                return;
            }

            Debug.Log($"Player {name} hit for {amount}");

            _health -= amount;
            if(IsDead) {
                Debug.Log($"Player {name} is dead!");

                _health = 0;

                _deathEffect.Trigger();

                GameManager.Instance.GameOver();
            } else {
                _hitEffect.Trigger(() => GamePlayerBehavior.OnIdle());
            }

            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdatePlayerHealthPercent(HealthPercent);
        }

        #region Actions

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(action is AttackAction) {
                if(!CanAttack) {
                    return true;
                }

                _attackEffect.Trigger(() => GamePlayerBehavior.OnIdle());

                return true;
            }

            if(action is StrongAttackAction) {
                if(!CanAttack) {
                    return true;
                }

                _strongAttackEffect.Trigger();
                _isStrongAttacking = true;

                return true;
            }

            return false;
        }

        public override bool OnCancelled(CharacterBehaviorAction action)
        {
            if(action is StrongAttackAction) {
                _strongAttackEffect.StopTrigger();
                _isStrongAttacking = false;

                GamePlayerBehavior.OnIdle();

                return true;
            }

            return false;
        }

        #endregion

        #region Animation Events

        public void OnAttackAnimationEvent(string name)
        {
            if(GamePlayerBehavior.GamePlayerBehaviorData.AttackSpawnVolumeEvent == name) {
                Debug.LogWarning("SPAWN ATTACK VOLUME");
            } else if(GamePlayerBehavior.GamePlayerBehaviorData.AttackDeSpawnVolumeEvent == name) {
                Debug.LogWarning("DESPAWN ATTACK VOLUME");
            } else {
                Debug.LogWarning($"Unhandled attack event: {name}");
            }
        }

        public void OnStrongAttackAnimationEvent(string name)
        {
            if(GamePlayerBehavior.GamePlayerBehaviorData.AttackSpawnVolumeEvent == name) {
                Debug.LogWarning("SPAWN STRONG ATTACK VOLUME");
            } else if(GamePlayerBehavior.GamePlayerBehaviorData.AttackDeSpawnVolumeEvent == name) {
                Debug.LogWarning("DESPAWN STRONG ATTACK VOLUME");
            } else {
                Debug.LogWarning($"Unhandled attack event: {name}");
            }
        }

        public void OnHitAnimationEvent(string name)
        {
            if(GamePlayerBehavior.GamePlayerBehaviorData.HitImmunityEvent == name) {
                _isImmune = !_isImmune;
            } else {
                Debug.LogWarning($"Unhandled hit event: {name}");
            }
        }

        #endregion

        #region Events

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            _health = MaxHealth;
            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdatePlayerHealthPercent(HealthPercent);

            return false;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            _health = MaxHealth;
            GameUIManager.Instance.GameGameUI.PlayerHUD.UpdatePlayerHealthPercent(HealthPercent);

            return false;
        }

        #endregion
    }
}
