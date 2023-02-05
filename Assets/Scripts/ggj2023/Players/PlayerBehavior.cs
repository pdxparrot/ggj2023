using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.ggj2023.Data.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.Players
{
    [RequireComponent(typeof(BeaverBehavior))]
    public sealed class PlayerBehavior : Game.Characters.Players.PlayerBehavior
    {
        public PlayerBehaviorData GamePlayerBehaviorData => (PlayerBehaviorData)PlayerBehaviorData;

        public BeaverBehavior BeaverBehavior { get; private set; }

        public override bool CanMove => base.CanMove && !BeaverBehavior.IsDead && !BeaverBehavior.IsStrongAttacking;

        public override bool IsAlive => base.IsAlive && !BeaverBehavior.IsDead;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            BeaverBehavior = GetComponent<BeaverBehavior>();
        }

        #endregion

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Player);
            Assert.IsTrue(behaviorData is PlayerBehaviorData);

            base.Initialize(behaviorData);

            Assert.IsTrue(HasBehaviorComponent<BeaverBehavior>(), "BeaverBehavior must be added to the behavior components!");
        }
    }
}
