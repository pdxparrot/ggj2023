using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Level;
using pdxpartyparrot.ggj2023.NPCs;
using pdxpartyparrot.ggj2023.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023.Levels
{
    public sealed class Level : LevelHelper
    {
        // TODO: NPCManager should handle this
        [CanBeNull]
        private GameObject _enemyContainer;

        [SerializeField]
        private Collider2D _viewerBounds;

        [SerializeField]
        [ReadOnly]
        private BlackberryMonster _boss;

        public BlackberryMonster Boss => _boss;

        private readonly HashSet<Vine> _vines = new HashSet<Vine>();

        public IReadOnlyCollection<Vine> Vines => _vines;

        private DebugMenuNode _debugMenuNode;

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.RegisterLevel(this);
        }

        protected override void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.UnRegisterLevel(this);
            }

            base.OnDestroy();
        }

        private void OnEnable()
        {
            InitDebugMenu();
        }

        private void OnDisable()
        {
            DestroyDebugMenu();
        }

        #endregion

        private void SpawnEnemies(Transform container)
        {
            SpawnBoss(container);
            SpawnVines(container);
        }

        private void SpawnBoss(Transform container)
        {
            SpawnPoint bossSpawnPoint = SpawnManager.Instance.GetSpawnPoint(GameManager.Instance.GameGameData.BossSpawnTag);

            Debug.Log($"Spawning boss");

            bossSpawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.BossPrefab, GameManager.Instance.GameGameData.BossBehaviorData, container);
        }

        private void SpawnVines(Transform container)
        {
            IReadOnlyCollection<SpawnPoint> spawnPoints = SpawnManager.Instance.GetSpawnPoints(GameManager.Instance.GameGameData.VineSpawnTag);

            Debug.Log($"Spawning vines from {spawnPoints.Count} spawners");

            foreach(SpawnPoint spawnPoint in spawnPoints) {
                spawnPoint.SpawnNPCPrefab(GameManager.Instance.GameGameData.VinePrefab, GameManager.Instance.GameGameData.VineBehaviorData, container);
            }
        }

        public void RegisterBoss(BlackberryMonster boss)
        {
            Assert.IsNull(_boss);
            _boss = boss;
        }

        public void UnRegisterBoss(BlackberryMonster boss)
        {
            Assert.AreEqual(_boss, boss);
            _boss = null;
        }

        public void RegisterVine(Vine vine)
        {
            Assert.IsTrue(_vines.Add(vine));
        }

        public void UnRegisterVine(Vine vine)
        {
            Assert.IsTrue(_vines.Remove(vine));
        }

        #region Event Handlers

        protected override void GameReadyEventHandler(object sender, EventArgs args)
        {
            base.GameReadyEventHandler(sender, args);

            Assert.IsNull(_enemyContainer);
            _enemyContainer = new GameObject("Enemies");

            SpawnEnemies(_enemyContainer.transform);

            GameManager.Instance.Viewer.SetBounds(_viewerBounds);

            GameManager.Instance.LevelEntered();
        }

        #endregion

        #region Debug Menu

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2023.Level");
            _debugMenuNode.RenderContentsAction = () => {
                GUI.enabled = !GameManager.Instance.IsGameOver && !PlayerManager.Instance.Player.GamePlayerBehavior.BeaverBehavior.IsDead;
                if(GUILayout.Button("Hit Player")) {
                    PlayerManager.Instance.Player.GamePlayerBehavior.BeaverBehavior.Damage(10);
                }
                if(GUILayout.Button("Kill Player")) {
                    PlayerManager.Instance.Player.GamePlayerBehavior.BeaverBehavior.Kill();
                }
                GUI.enabled = true;

                GUI.enabled = !GameManager.Instance.IsGameOver && Vines.Any();
                if(GUILayout.Button("Hit Vine")) {
                    Vines.ElementAt(0).VineBehavior.Damage(10);
                }
                if(GUILayout.Button("Kill Vine")) {
                    Vines.ElementAt(0).VineBehavior.Kill();
                }
                GUI.enabled = true;

                GUI.enabled = !GameManager.Instance.IsGameOver && Boss != null;
                if(GUILayout.Button("Hit Boss")) {
                    Boss.BlackberryMonsterBehavior.Damage(10);
                }
                if(GUILayout.Button("Kill Boss")) {
                    Boss.BlackberryMonsterBehavior.Kill();
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
