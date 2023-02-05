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

        [Space(10)]

        [SerializeField]
        private string _attackSpawnVolumeEvent = "attack_spawnvolume";

        public string AttackSpawnVolumeEvent => _attackSpawnVolumeEvent;

        [SerializeField]
        private string _attackDeSpawnVolumeEvent = "attack_despawnvolume";

        public string AttackDeSpawnVolumeEvent => _attackDeSpawnVolumeEvent;

        // TODO: VFX / hit impact?

        [SerializeField]
        private string _hitImmunityEvent = "immunity";

        public string HitImmunityEvent => _hitImmunityEvent;
    }
}
