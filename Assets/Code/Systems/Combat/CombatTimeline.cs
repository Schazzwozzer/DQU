using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DQU.Configurations;

namespace DQU.Combat
{
    /// <summary>
    /// Tracks pending combat Actions, ordering them according to when they should execute.
    /// </summary>
    public sealed class CombatTimeline
    {
        // The sorted list of all pending Actions, sorted in the order in which they should execute.
        private List<ActionEntry> _actions = new List<ActionEntry>();
        
        // Used to sort of the main list.
        private TimelineComparer _comparer = new TimelineComparer();

        // The Actions which should execute during the current turn.
        private Queue<ActionEntry> _readyActions = new Queue<ActionEntry>();

        /// <summary>
        /// Are there Actions waiting to be executed on future turns?
        /// </summary>
        public bool HasPendingActions { get { return _actions.Count > 0; } }

        /// <summary>
        /// Are there Actions which ready to execute during this turn?
        /// </summary>
        public bool HasReadyActions { get { return _readyActions.Count > 0; } }

        /// <summary>
        /// The Action which should presently be executed.
        /// </summary>
        public ISelfActionInstance Current { get; private set; }


        /// <summary>
        /// Add an instantiated Action to the Timeline.
        /// </summary>
        public void Add( ISelfActionInstance action )
        {
            _actions.Add( new ActionEntry( action ) );
            _actions.Sort( _comparer );
        }

        /// <summary>
        /// <para>Process the next Combat step.</para>
        /// That will either be to make the next ready Action available as <see cref="Current"/>
        /// or to tick the countdown clocks on all of the pending Actions.
        /// </summary>
        /// <returns>
        /// True if a new combat turn was begun (the countdown clocks were ticked);
        /// false if a ready Action was processed instead.
        /// </returns>
        public bool MoveNext()
        {
            if( _actions.Count == 0 )
                return false;

            bool turnTaken;
            Current = null;

            if( !HasReadyActions )
            {
                // Decrease each Action's remaining time by 1.
                for( int i = 0; i < _actions.Count; ++i )
                {
                    if( _actions[i].Tick() )
                        // If the Action has no remaining time,
                        // add it to the Actions-Ready-to-Fire queue.
                        _readyActions.Enqueue( _actions[i] );
                }
                turnTaken = true;
            }
            else
            {
                ActionEntry entry = _readyActions.Dequeue();
                Current = entry.Action;
                _actions.Remove( entry );

                turnTaken = false;
            }
            
            return turnTaken;
        }

        /// <summary>
        /// Remove all currently tracked Actions—including ready Actions.
        /// </summary>
        public void Clear()
        {
            _actions.Clear();
            _readyActions.Clear();
            Current = null;
        }


        /// <summary>
        /// Data class describing a specific Action instance in the timeline.
        /// </summary>
        public class ActionEntry
        {
            /// <summary>The Action to be queued and executed.</summary>
            public ISelfActionInstance Action { get; private set; }

            /// <summary>
            /// The 'countdown clock' which tracks how many Combat turns until this Action should execute.
            /// </summary>
            public int RemainingTime { get; private set; }

            /// <summary>Does this Action originate from the Player Actor?</summary>
            public bool PlayerIsSource { get { return Action.SourceActor is PlayerState; } }

            public ActionEntry( ISelfActionInstance action )
            {
                this.Action = action;
                this.RemainingTime = action.Configuration.Time;
            }

            /// <summary>
            /// Decrease the Action's remaining time by 1.
            /// </summary>
            /// <returns>True if the timer has reached 0; false if not.</returns>
            public bool Tick()
            {
                RemainingTime -= 1;
                return RemainingTime <= 0;
            }
        }


        /// <summary>
        /// Maintains the Timeline's order, based on each Action's 
        /// remaining time, with precedence given to the Player.
        /// </summary>
        private class TimelineComparer : IComparer<ActionEntry>
        {
            public int Compare( ActionEntry x, ActionEntry y )
            {
                // Negate the result because we want lower values to go first.
                int result = -x.RemainingTime.CompareTo( y.RemainingTime );
                if( result == 0 )
                    return x.PlayerIsSource.CompareTo( y.PlayerIsSource );
                else
                    return result;
            }
        }

    }
}
