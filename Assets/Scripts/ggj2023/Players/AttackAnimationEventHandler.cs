using pdxpartyparrot.Core.Animation;

using Spine;

namespace pdxpartyparrot.ggj2023.Players
{
    public sealed class AttackAnimationEventHandler : SpineAnimationEventHandler
    {
        protected override void AnimationEvent(TrackEntry trackEntry, Event evt)
        {
            PlayerManager.Instance.Player.GamePlayerBehavior.BeaverBehavior.OnAttackAnimationEvent(evt.Data.Name);
        }
    }
}
