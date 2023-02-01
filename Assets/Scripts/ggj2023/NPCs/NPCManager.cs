using System.Collections.Generic;

using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.NPCs;
using pdxpartyparrot.ggj2023.Data.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.NPCs
{
    public sealed class NPCManager : NPCManager<NPCManager>
    {
        [SerializeField]
        private BlackberryMonsterData _bossData;

        public BlackberryMonsterData BossData => _bossData;

        [SerializeField]
        private VineData _vineData;

        public VineData VineData => _vineData;

        public void SpawnEnemies(Transform container)
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
    }
}
