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

        #region Actions

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(action is AttackAction) {
                Debug.Log("Attack!");

                return true;
            }

            if(action is StrongAttackAction) {
                Debug.Log("STRONG attack!");

                return true;
            }

            return false;
        }

        #endregion
    }
}
