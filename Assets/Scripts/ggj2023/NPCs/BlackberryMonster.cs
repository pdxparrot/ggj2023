using System;

using pdxpartyparrot.Game.Characters.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.NPCs
{
    public sealed class BlackberryMonster : NPC25D
    {
        public BlackberryMonsterBehavior BlackberryMonsterBehavior => (BlackberryMonsterBehavior)NPCBehavior;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

            SetPassive();
        }

        protected override void OnDestroy()
        {
            if(NPCManager.HasInstance) {
                NPCManager.Instance.UnregisterNPC(this);
            }

            base.OnDestroy();
        }

        #endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            Assert.IsTrue(Behavior is BlackberryMonsterBehavior);
        }
    }
}
