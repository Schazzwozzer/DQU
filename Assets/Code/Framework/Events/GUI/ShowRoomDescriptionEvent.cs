using SDD.Events;
using UnityEngine.Localization;

namespace DQU.Events
{
    /// <summary>
    /// EVENT: "Show Room Description".
    /// Notifies that a Room Description should be displayed on the screen.
    /// <para>Used to pass info about the description string to the gameobjects which require it.</para>
    /// </summary>
    public class ShowRoomDescriptionsEvent : Event
    {
        public LocalizedString RoomDescription { get; private set; }

        /// <summary>
        /// The number of seconds that should transpire before the Description begins displaying.
        /// </summary>
        public float Delay { get; private set; }

        public RoomDescriptionLayout TextLayout { get; private set; }

        public ShowRoomDescriptionsEvent Configure( 
            LocalizedString roomDescription, 
            float secondsDelay,
            RoomDescriptionLayout layout )
        {
            this.RoomDescription = roomDescription;
            this.Delay = secondsDelay;
            this.TextLayout = layout;

            return this;
        }

#if DEBUG
        public override void Log()
        {
            //UnityEngine.Debug.Log( "EVENT: " + ToString() );
        }
#endif

        public override string ToString()
        {
            return string.Format( "Show Room Description ({0})", RoomDescription.TableEntryReference.KeyId );
        }

    }
}