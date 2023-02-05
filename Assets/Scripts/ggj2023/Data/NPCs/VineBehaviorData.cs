using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Data.NPCs
{
    [CreateAssetMenu(fileName = "VineBehaviorData", menuName = "pdxpartyparrot/ggj2023/Data/NPCs/VineBehavior Data")]
    [Serializable]
    public sealed class VineBehaviorData : Game.Data.Characters.NPCBehaviorData
    {
        [Space(10)]

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
