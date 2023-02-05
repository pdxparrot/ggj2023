using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Data.Players
{
    [CreateAssetMenu(fileName = "PlayerBehaviorData", menuName = "pdxpartyparrot/ggj2023/Data/Players/PlayerBehavior Data")]
    [Serializable]
    public sealed class PlayerBehaviorData : Game.Data.Characters.PlayerBehaviorData
    {
        [Space(10)]

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
