using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2023.Data.NPCs
{
    [CreateAssetMenu(fileName = "BlackberryMonsterBehaviorData", menuName = "pdxpartyparrot/ggj2023/Data/NPCs/BlackberryMonsterBehavior Data")]
    [Serializable]
    public sealed class BlackberryMonsterBehaviorData : Game.Data.Characters.NPCBehaviorData
    {
        [Space(10)]

        [SerializeField]
        private int _maxHealth = 100;

        public int MaxHealth => _maxHealth;
    }
}
