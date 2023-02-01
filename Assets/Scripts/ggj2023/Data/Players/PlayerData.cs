using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Data.Players
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "pdxpartyparrot/ggj2023/Data/Players/Player Data")]
    [Serializable]
    public sealed class PlayerData : Game.Data.Players.PlayerData
    {
        [SerializeField]
        private int _maxHealth = 100;

        public int MaxHealth => _maxHealth;

        [SerializeField]
        private int _attackDamage = 10;

        public int AttackDamage => _attackDamage;

        [SerializeField]
        private int _strongAttackDamage = 20;

        public int StrongAttackDamage => _strongAttackDamage;
    }
}
