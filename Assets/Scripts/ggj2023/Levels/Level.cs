using System;

using JetBrains.Annotations;

using pdxpartyparrot.Game.Level;
using pdxpartyparrot.ggj2023.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.Level
{
    public sealed class Level : LevelHelper
    {
        // TODO: NPCManager should handle this
        [CanBeNull]
        private GameObject _enemyContainer;

        [SerializeField]
        private Collider2D _viewerBounds;

        #region Event Handlers

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            Assert.IsNull(_enemyContainer);
            _enemyContainer = new GameObject("Enemies");

            NPCManager.Instance.SpawnEnemies(_enemyContainer.transform);

            GameManager.Instance.Viewer.SetBounds(_viewerBounds);

            GameManager.Instance.LevelEntered();
        }

        #endregion
    }
}
