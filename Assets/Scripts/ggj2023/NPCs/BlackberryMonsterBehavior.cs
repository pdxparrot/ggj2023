using System.Linq;

using pdxpartyparrot.Core.Data.Actors.Components;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2023.Data.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.NPCs
{
    public sealed class BlackberryMonsterBehavior : NPCBehavior
    {
        private BlackberryMonster BlackberryMonster => (BlackberryMonster)Owner;

        private BlackberryMonsterBehaviorData BlackberryMonsterBehaviorData => (BlackberryMonsterBehaviorData)BehaviorData;

        public override Vector3 MoveDirection => BlackberryMonster.MoveDirection;

        public override void Initialize(ActorBehaviorComponentData behaviorData)
        {
            Assert.IsTrue(Owner is BlackberryMonster);
            Assert.IsTrue(behaviorData is BlackberryMonsterBehaviorData);

            base.Initialize(behaviorData);
        }
    }
}
