#if USE_SPINE
using System.Collections.Generic;

using pdxpartyparrot.Core.Effects.EffectTriggerComponents;

using Spine;

using UnityEngine;

namespace pdxpartyparrot.Core.Animation
{
    // TODO: should this actually go with the effect trigger components?
    public abstract class SpineAnimationEventHandler : MonoBehaviour
    {
        [SerializeField]
        private SpineAnimationEffectTriggerComponent _animationEffectTriggerComponent;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            _animationEffectTriggerComponent.StartEvent += AnimationStartHandler;
            _animationEffectTriggerComponent.CompleteEvent += AnimationCompleteHandler;
        }

        protected virtual void OnDesteroy()
        {
            _animationEffectTriggerComponent.StartEvent -= AnimationStartHandler;
            _animationEffectTriggerComponent.CompleteEvent -= AnimationCompleteHandler;
        }

        #endregion

        private void AnimationStartHandler(object sender, SpineAnimationEffectTriggerComponent.EventArgs args)
        {
            args.TrackEntry.Event += AnimationEvent;
        }

        private void AnimationCompleteHandler(object sender, SpineAnimationEffectTriggerComponent.EventArgs args)
        {
            args.TrackEntry.Event -= AnimationEvent;
        }

        protected abstract void AnimationEvent(TrackEntry trackEntry, Spine.Event evt);
    }
}
#endif
