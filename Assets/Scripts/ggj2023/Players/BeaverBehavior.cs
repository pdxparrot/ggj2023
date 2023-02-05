using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.Players.BehaviorComponents;
using pdxpartyparrot.ggj2023.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Players
{
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

        [Space(10)]

        [SerializeField]
        private EffectTrigger _attackEffect;

        [SerializeField]
        private EffectTrigger _strongAttackEffect;

        [SerializeField]
        private EffectTrigger _hitEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        #endregion

        [Space(10)]

        [SerializeField]
        private BeaverDamageVolume _damageVolume;

        #region Health

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private int _health;

        public int Health => _health;

        public int MaxHealth => GamePlayerBehavior.GamePlayerBehaviorData.MaxHealth;

        private float HealthPercent => Mathf.Clamp(Health / (float)MaxHealth, 0.0f, 1.0f);

        public bool IsDead => Health <= 0;

        [SerializeField]
        [ReadOnly]
        private bool _isImmune;

        public bool IsImmune => PlayerManager.Instance.PlayersImmune || _isImmune;

        #endregion

        #region Attack

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private bool _isAttackingDamage;

        public bool IsAttacking => _attackEffect.IsRunning;

        #endregion

        #region Strong Attack

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private bool _isStrongAttacking;

        public bool IsStrongAttacking => _isStrongAttacking;

        private ITimer _strongAttackDamageTimer;

        #endregion

        private bool CanAttack => !IsDead && !IsAttacking && !IsStrongAttacking;

        #region Unity Lifecycle

        private void OnEnable()
        {
            _strongAttackDamageTimer = TimeManager.Instance.AddTimer();
        }

        private void OnDisable()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_strongAttackDamageTimer);
                _strongAttackDamageTimer = null;
            }
        }

        private void Update()
        {
            if(_isAttackingDamage) {
                _damageVolume.Damage(GamePlayerBehavior.GamePlayerBehaviorData.AttackDamage);
            }

            // TODO: the cooldown doesn't seem to be working
            if(IsStrongAttacking && !_strongAttackDamageTimer.IsRunning) {
                _damageVolume.StrongDamage(GamePlayerBehavior.GamePlayerBehaviorData.StrongAttackDamage);

                _strongAttackDamageTimer.StartMillis(GamePlayerBehavior.GamePlayerBehaviorData.StrongAttackDamageCooldownMillis);
            }
        }

        #endregion

        public void Kill()
        {
            if(IsDead || IsImmune) {
                return;
            }

            Debug.Log($"Killing player {name}!");

            Damage(_health);
        }

        public void Damage(int amount)
        {
            if(IsDead || IsImmune) {
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
                // TODO: not sure what's wrong here
                // but we want the anim / audio to finish here
                // (might be that we want to remove looping and loop it manually
                // while the button is being held down?)
                //_strongAttackEffect.StopTrigger();
                _strongAttackEffect.KillTrigger();

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
                _damageVolume.ResetDamaged();
                _isAttackingDamage = true;
            } else if(GamePlayerBehavior.GamePlayerBehaviorData.AttackDeSpawnVolumeEvent == name) {
                _isAttackingDamage = false;
                _damageVolume.ResetDamaged();
            } else {
                Debug.LogWarning($"Unhandled attack event: {name}");
            }
        }

        public void OnStrongAttackAnimationEvent(string name)
        {
            // NOTE: ignoring the damage volume with this because looping screws it up
            if(GamePlayerBehavior.GamePlayerBehaviorData.AttackSpawnVolumeEvent == name) {
                //Debug.LogWarning("SPAWN STRONG ATTACK VOLUME");
            } else if(GamePlayerBehavior.GamePlayerBehaviorData.AttackDeSpawnVolumeEvent == name) {
                ///Debug.LogWarning("DESPAWN STRONG ATTACK VOLUME");
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
