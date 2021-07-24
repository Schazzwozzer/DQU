using UnityEngine;
using System;

namespace DQU.Configurations
{
    /// <summary>
    /// A simple helper class that pairs an Actor and a 
    /// collection of Effects that should be applied to it.
    /// <para>Includes a method to apply those Effects, <see cref="ApplyEffects"/>.</para>
    /// </summary>
    public class ActionTargetAndEffects
    {
        public ActorStateBase Target { get; private set; }

        public ActionEffect[] Effects { get; private set; }

        public ActionTargetAndEffects( ActorStateBase target, ActionEffect[] effects )
        {
            Target = target;
            Effects = effects;
        }

        /// <summary>
        /// Iterate through this struct's Effects
        /// and apply each one to the target Actor.
        /// </summary>
        public void ApplyEffects()
        {
            for( int i = 0; i < Effects.Length; ++i )
                Target.ApplyEffect( Effects[i] );
        }

    }
}
