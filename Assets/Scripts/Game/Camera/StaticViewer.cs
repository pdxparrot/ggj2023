using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;

namespace pdxpartyparrot.Game.Camera
{
    // TODO: could this inherit from CinemachineViewer and be ok?
    // that would give us impulses and such
    public class StaticViewer : Viewer, IPlayerViewer
    {
        public Viewer Viewer => this;

        public virtual void Initialize(GameData gameData)
        {
            Viewer.Set3D(gameData.FoV);
        }
    }
}
