using UnityEngine;
using SDD.Events;

namespace DQU.Events
{
    /// <summary>
    /// EVENT: "Shadow Color Changed".
    /// Notifies that the environment's shadow color has been changed.
    /// </summary>
    public class ShadowColorChangedEvent : SDD.Events.Event
    {
        public Color ShadowColor { get; private set; }

        public ShadowColorChangedEvent( Color shadowColor )
        {
            this.ShadowColor = shadowColor;
        }

#if DEBUG
        public override void Log()
        {
            //UnityEngine.Debug.Log( "EVENT: " + ToString() );
        }
#endif

        public override string ToString()
        {
            return "Shadow Color Changed (" + ShadowColor.ToString() + ")";
        }

    }
}