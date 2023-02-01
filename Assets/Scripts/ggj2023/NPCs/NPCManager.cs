using System.Collections.Generic;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.NPCs;
using pdxpartyparrot.ggj2023.Data.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

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

        [SerializeField]
        [ReadOnly]
        private BlackberryMonster _boss;

        public BlackberryMonster Boss => _boss;

        private readonly HashSet<Vine> _vines = new HashSet<Vine>();

        public IReadOnlyCollection<Vine> Vines => _vines;


        public override void RegisterNPC(INPC npc)
        {
            base.RegisterNPC(npc);

            if(npc is BlackberryMonster) {
                Assert.IsNull(_boss);
                _boss = npc as BlackberryMonster;
            } else if(npc is Vine) {
                _vines.Add(npc as Vine);
            }
        }

        public override void UnregisterNPC(INPC npc)
        {
            if(npc is BlackberryMonster) {
                Assert.IsNotNull(_boss);
                _boss = null;
            } else if(npc is Vine) {
                _vines.Remove(npc as Vine);
            }

            base.UnregisterNPC(npc);
        }

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
