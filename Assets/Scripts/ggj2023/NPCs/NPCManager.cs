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
    }
}
