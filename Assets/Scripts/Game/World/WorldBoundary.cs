using UnityEngine;
#if USE_NAVMESH
using Unity.AI.Navigation;
#endif

namespace pdxpartyparrot.Game.World
{
#if USE_NAVMESH
    [RequireComponent(typeof(NavMeshModifier))]
#endif
    public abstract class WorldBoundary : MonoBehaviour
    {
        protected void HandleCollisionEnter(GameObject go)
        {
            IWorldBoundaryCollisionListener listener = go.GetComponent<IWorldBoundaryCollisionListener>();
            if(null == listener) {
                return;
            }

            listener.OnWorldBoundaryCollisionEnter(this);
        }

        protected void HandleCollisionExit(GameObject go)
        {
            IWorldBoundaryCollisionListener listener = go.GetComponent<IWorldBoundaryCollisionListener>();
            if(null == listener) {
                return;
            }

            listener.OnWorldBoundaryCollisionExit(this);
        }
    }
}
