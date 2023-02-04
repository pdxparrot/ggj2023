using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Game.Characters.Players.BehaviorComponents;

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

        [SerializeField]
        private EffectTrigger _attackEffect;

        [SerializeField]
        private EffectTrigger _strongAttackEffect;

        [SerializeField]
        private RumbleEffectTriggerComponent[] _rumbleEffects;

        #endregion

        private bool CanAttack => !_attackEffect.IsRunning && !_strongAttackEffect.IsRunning;

        public void Initialize()
        {
            foreach(RumbleEffectTriggerComponent rumble in _rumbleEffects) {
                rumble.PlayerInput = Owner.PlayerInputHandler.InputHelper;
            }
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

                _strongAttackEffect.Trigger(() => GamePlayerBehavior.OnIdle());

                return true;
            }

            return false;
        }

        #endregion
    }
}
