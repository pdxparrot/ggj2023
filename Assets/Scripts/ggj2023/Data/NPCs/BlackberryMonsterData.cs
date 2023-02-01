using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Data.NPCs
{
    [CreateAssetMenu(fileName = "BlackberryMonsterData", menuName = "pdxpartyparrot/ggj2023/Data/NPCs/BlackberryMonster Data")]
    [Serializable]
    public sealed class BlackberryMonsterData : Game.Data.NPCs.NPCData
    {
        [SerializeField]
        private int _maxHealth = 100;

        public int MaxHealth => _maxHealth;
    }
}
