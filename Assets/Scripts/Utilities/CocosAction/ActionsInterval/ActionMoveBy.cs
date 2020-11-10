using System;
using System.Collections.Generic;
using UnityEngine;

namespace CocosAction
{
    /// <summary>
    /// Moves in the given direction for the given amount of seconds.
    /// </summary>
    class ActionMoveBy : ActionInterval
    {
        protected Vector3 delta;

        public ActionMoveBy(Vector3 targetDelta, float targetDuration)
            : base(targetDuration)
        {
            delta = targetDelta;
        }

        public ActionMoveBy(Vector2 targetValue, float targetDuration)
            : this((Vector3)targetValue, targetDuration)
        {
        }

        public override ActionInstant Clone()
        {
            return new ActionMoveBy(delta, duration);
        }

        public override ActionInstant Reverse()
        {
            return new ActionMoveBy(delta * -1F, duration);
        }

        public override void Step(float dt)
        {
            float d = dt / duration;
            transform.Translate(delta * d, Space.World);
        }
    }
}