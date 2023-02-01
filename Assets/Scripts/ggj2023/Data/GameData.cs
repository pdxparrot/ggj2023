using System;

using pdxpartyparrot.ggj2023.Camera;
using pdxpartyparrot.ggj2023.Data.NPCs;
using pdxpartyparrot.ggj2023.NPCs;

using UnityEngine;


namespace pdxpartyparrot.ggj2023.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "pdxpartyparrot/ggj2023/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        public GameViewer GameViewerPrefab => (GameViewer)ViewerPrefab;

        [Space(10)]

        [Header("NPCs")]

        [SerializeField]
        private string _bossSpawnTag = "boss";

        public string BossSpawnTag => _bossSpawnTag;

        [SerializeField]
        private BlackberryMonster _bossPrefab;

        public BlackberryMonster BossPrefab => _bossPrefab;

        [SerializeField]
        private BlackberryMonsterBehaviorData _bossBehaviorData;

        public BlackberryMonsterBehaviorData BossBehaviorData => _bossBehaviorData;

        [Space(10)]

        [SerializeField]
        private string _vineSpawnTag = "vine";

        public string VineSpawnTag => _vineSpawnTag;

        [SerializeField]
        private Vine _vinePrefab;

        public Vine VinePrefab => _vinePrefab;

        [SerializeField]
        private VineBehaviorData _vineBehaviorData;

        public VineBehaviorData VineBehaviorData => _vineBehaviorData;
    }
}
