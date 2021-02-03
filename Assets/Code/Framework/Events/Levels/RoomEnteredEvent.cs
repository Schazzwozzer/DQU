using SDD.Events;

namespace DQU.Events
{
    /// <summary>
    /// EVENT: "Room Entered".
    /// Notifies that the Player has entered a Room's trigger collider.
    /// </summary>
    public class RoomEnteredEvent : Event
    {
        public Room Room { get; private set; }

        public RoomEnteredEvent Configure( Room room )
        {
            this.Room = room;

            return this;
        }

#if DEBUG
        public override void Log()
        {
            UnityEngine.Debug.Log( "EVENT: " + ToString() );
        }
#endif

        public override string ToString()
        {
            return Room.gameObject.name + " Entered.";
        }

    }
}