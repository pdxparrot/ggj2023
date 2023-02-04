using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2023.Data.Players;

namespace pdxpartyparrot.ggj2023.Players
{
    public sealed class PlayerManager : PlayerManager<PlayerManager>
    {
        public PlayerData GamePlayerData => (PlayerData)PlayerData;

        [CanBeNull]
        public Player Player => Players.ElementAt(0) as Player;
    }
}
