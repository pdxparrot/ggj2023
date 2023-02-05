using pdxpartyparrot.Game.UI;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.ggj2023.UI
{
    public sealed class PlayerHUD : HUD
    {
        [SerializeField]
        private Image _playerHealth;

        [SerializeField]
        private Image _bossHealth;

        public void UpdatePlayerHealthPercent(float pct)
        {
            _playerHealth.fillAmount = pct;
        }

        public void UpdateBossHealthPercent(float pct)
        {
            _bossHealth.fillAmount = pct;
        }
    }
}
