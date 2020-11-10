using System;
using System.Collections.Generic;
using UnityEngine;

namespace CocosAction
{
    /// <summary>
    /// This action stops all actions for the current target.
    /// </summary>
    class ActionStop : ActionInstant
    {
        public ActionStop()
            : base()
        {
        }

        public override ActionInstant Clone()
        {
            return new ActionStop();
        }

        public override void Start()
        {
            base.Start();
            target.StopAllActions();
        }
    }
}