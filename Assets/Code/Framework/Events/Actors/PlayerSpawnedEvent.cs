using UnityEngine;
using SDD.Events;

namespace DQU.Events
{
    /// <summary>
    /// EVENT: "Player Spawned".
    /// Notifies that a Player GameObject has been spawned into the Level.
    /// </summary>
    public class PlayerSpawnedEvent : SDD.Events.Event
    {
        public Transform Player { get; private set; }

        public PlayerSpawnedEvent( Transform player )
        {
            this.Player = player;
        }

#if DEBUG
        public override void Log()
        {
            //UnityEngine.Debug.Log( "EVENT: " + ToString() );
        }
#endif

        public override string ToString()
        {
            return "Player Spawned";
        }

    }
}