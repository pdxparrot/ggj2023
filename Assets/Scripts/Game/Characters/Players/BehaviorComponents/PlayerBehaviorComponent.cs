using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Game.Characters.BehaviorComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.Players.BehaviorComponents
{
    public abstract class PlayerBehaviorComponent : CharacterBehaviorComponent
    {
        protected PlayerBehavior PlayerBehavior => (PlayerBehavior)Behavior;

        [SerializeField]
        private RumbleEffectTriggerComponent[] _rumbleEffects;

        public override void Initialize(CharacterBehavior behavior)
        {
            Assert.IsTrue(behavior is PlayerBehavior);

            base.Initialize(behavior);

            Debug.Log($"Initializing {_rumbleEffects.Length} rumble effects on {name}...");
            foreach(RumbleEffectTriggerComponent rumble in _rumbleEffects) {
                rumble.PlayerInput = PlayerBehavior.Player.PlayerInputHandler.InputHelper;
            }
        }
    }
}
