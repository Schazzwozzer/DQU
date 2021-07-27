using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DQU.Configurations;

namespace DQU.Combat
{
    /// <summary>
    /// Handles the logic of combat, maintaining a timeline of 
    /// pending combat actions and executing them when appropriate.
    /// </summary>
    public static class CombatEngine
    {
        private static CombatTimeline _timeline;

        public static bool HasPendingActions { get { return _timeline.HasPendingActions; } }


        static CombatEngine()
        {
            _timeline = new CombatTimeline();
        }


        public static void BeginAction( ISelfActionInstance action )
        {
            _timeline.Add( action );
        }


        /// <summary>
        /// Moves the combat timeline along, either processing an action, or moving to the next turn.
        /// </summary>
        /// <returns>True if a new turn has been taken; false if an action was executed.</returns>
        public static bool Tick()
        {
            if( _timeline.MoveNext() )
            {
                // A new combat turn has begun.
                return true;
            }
            else
            {
                TriggerAction( _timeline.Current );
                return false;
            }
        }

        public static void TriggerAction( ISelfActionInstance action )
        {
            // Apply effects to self
            for( int i = 0; i < action.SourceEffects.Length; ++i )
                action.SourceActor.ApplyEffect( action.SourceEffects[i] );

            // Apply effects to target
            if( action is ITargetedActionInstance )
            {
                var targetAction = (ITargetedActionInstance)action;
                for( int i = 0; i < targetAction.TargetEffects.Length; ++i )
                    targetAction.TargetActor.ApplyEffect( targetAction.TargetEffects[i] );
            }
        }


        /// <summary>
        /// Remove all current combat Actions.
        /// </summary>
        public static void Reset()
        {
            _timeline.Clear();
        }

    }
}