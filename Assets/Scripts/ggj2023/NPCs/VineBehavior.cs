using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2023.Data.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.NPCs
{
    public sealed class VineBehavior : NPCBehavior
    {
        private Vine Vine => (Vine)Owner;

        private VineBehaviorData VineBehaviorData => (VineBehaviorData)BehaviorData;

        public override Vector3 MoveDirection => Vine.MoveDirection;

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is Vine);
            Assert.IsTrue(behaviorData is VineBehaviorData);

            base.Initialize(behaviorData);
        }
    }
}
