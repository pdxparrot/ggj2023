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

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Player);
            Assert.IsTrue(behaviorData is PlayerBehaviorData);

            base.Initialize(behaviorData);
        }
    }
}
