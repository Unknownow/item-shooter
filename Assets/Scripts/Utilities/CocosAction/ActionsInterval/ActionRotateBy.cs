﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CocosAction
{
    /// <summary>
    /// Rotates by the given euler angles.
    /// </summary>
    class ActionRotateBy : ActionInterval
    {
        protected Vector3 delta;

        public ActionRotateBy(Vector3 targetDelta, float targetDuration)
            : base(targetDuration)
        {
            delta = targetDelta;
        }

        public ActionRotateBy(float angle, float targetDuration)
            : this(new Vector3(0, 0, angle), targetDuration)
        {
            locks = Axises.rxy;
        }

        public override ActionInstant Clone()
        {
            return new ActionRotateBy(delta, duration);
        }

        public override ActionInstant Reverse()
        {
            return new ActionRotateBy(delta * -1F, duration);
        }

        public override void Step(float dt)
        {
            float d = dt / duration;
            Vector3 target = delta * d;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + target);
        }
    }
}