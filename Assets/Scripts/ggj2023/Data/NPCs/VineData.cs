using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Data.NPCs
{
    [CreateAssetMenu(fileName = "VineData", menuName = "pdxpartyparrot/ggj2023/Data/NPCs/Vine Data")]
    [Serializable]
    public sealed class VineData : Game.Data.NPCs.NPCData
    {
        [SerializeField]
        private int _maxHealth = 50;

        public int MaxHealth => _maxHealth;

        [SerializeField]
        private int _attackDamage = 10;

        public int AttackDamage => _attackDamage;

        [SerializeField]
        private int _deathDamage = 10;

        public int DeathDamage => _deathDamage;
    }
}
