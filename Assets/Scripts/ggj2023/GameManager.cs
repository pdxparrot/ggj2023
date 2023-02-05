using UnityEngine;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game;
using pdxpartyparrot.ggj2023.Camera;
using pdxpartyparrot.ggj2023.Data;
using pdxpartyparrot.ggj2023.Levels;

using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2023
{
    public sealed class GameManager : GameManager<GameManager>
    {
        public GameData GameGameData => (GameData)GameData;

        public GameViewer Viewer { get; private set; }

        [SerializeField]
        [ReadOnly]
        private Level _currentLevel;

        public Level CurrentLevel => _currentLevel;

        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>();
            if(null == Viewer) {
                Debug.LogWarning("Unable to acquire game viewer!");
                return;
            }
            Viewer.Initialize(GameGameData);
        }

        public void RegisterLevel(Level level)
        {
            Assert.IsNull(_currentLevel);
            _currentLevel = level;
        }

        public void UnRegisterLevel(Level level)
        {
            Assert.AreEqual(_currentLevel, level);
            _currentLevel = null;
        }
    }
}
