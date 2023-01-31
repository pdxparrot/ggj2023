using pdxpartyparrot.Game.Characters.BehaviorComponents;
using pdxpartyparrot.Game.Players.Input;
using pdxpartyparrot.ggj2023.Data.Players;

using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace pdxpartyparrot.ggj2023.Players
{
    public sealed class PlayerInputHandler : ThirdPersonPlayerInputHandler
    {
        private Player GamePlayer => (Player)Player;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerInputData is PlayerInputData);
            Assert.IsTrue(Player is Player);
        }

        #endregion

        #region Actions

        public void OnJumpAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            if(context.performed) {
                GamePlayer.PlayerBehavior.ActionPerformed(JumpBehaviorComponent.JumpAction.Default);
            }
        }

        public void OnAttackAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            if(context.performed) {
                //GTODO: amePlayer.PlayerBehavior.ActionPerformed(BeaverBehaviorComponent.AttackAction.Default);
            }
        }

        public void OnStrongAttackAction(InputAction.CallbackContext context)
        {
            if(!IsInputAllowed(context)) {
                return;
            }

            if(context.performed) {
                //TODO: GamePlayer.PlayerBehavior.ActionPerformed(BeaverBehaviorComponent.StrongAttackAction.Default);
            }
        }

        #endregion
    }
}
