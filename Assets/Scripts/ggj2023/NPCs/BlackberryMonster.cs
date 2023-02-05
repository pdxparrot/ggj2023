using System;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.Interactables;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.NPCs
{
    public sealed class BlackberryMonster : NPC25D, IInteractable
    {
        public BlackberryMonsterBehavior BlackberryMonsterBehavior => (BlackberryMonsterBehavior)NPCBehavior;

        public bool CanInteract => !BlackberryMonsterBehavior.IsDead;

        public Type InteractableType => GetType();

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

            SetPassive();
        }

        protected override void OnDestroy()
        {
            // TODO: not really sure this is legit necessary
            // it's called from OnDeSpawn
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

        #region Spawn

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            GameManager.Instance.CurrentLevel.RegisterBoss(this);

            return true;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnReSpawn(spawnpoint)) {
                return false;
            }

            GameManager.Instance.CurrentLevel.RegisterBoss(this);

            return true;
        }

        public override void OnDeSpawn()
        {
            GameManager.Instance.CurrentLevel.UnRegisterBoss(this);

            base.OnDeSpawn();
        }

        #endregion
    }
}
