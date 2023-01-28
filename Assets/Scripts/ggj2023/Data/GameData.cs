using System;

using UnityEngine;

using pdxpartyparrot.ggj2023.Camera;

namespace pdxpartyparrot.ggj2023.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "pdxpartyparrot/ggj2023/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        public GameViewer GameViewerPrefab => (GameViewer)ViewerPrefab;
    }
}
