using System;
using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
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

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        private void OnEnable()
        {
            InitDebugMenu();
        }

        private void OnDisable()
        {
            DestroyDebugMenu();
        }

        #endregion

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

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2023.Level");
            _debugMenuNode.RenderContentsAction = () => {
                GUI.enabled = NPCManager.Instance.Vines.Any();
                if(GUILayout.Button("Kill Vine")) {
                    NPCManager.Instance.Vines.ElementAt(0).Kill();
                }
                GUI.enabled = true;

                GUI.enabled = NPCManager.Instance.Boss != null;
                if(GUILayout.Button("Kill Boss")) {
                    NPCManager.Instance.Boss.Kill();
                }
                GUI.enabled = true;
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }

        #endregion
    }
}
