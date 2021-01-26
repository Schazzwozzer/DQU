namespace SDD.Events {

  /// <summary>
  /// Base event for all EventManager events.
  /// </summary>
  public class Event {

    /// <summary>
    /// Called when the event is raised.
    /// Override to insert logging functionality.
    /// </summary>
    public virtual void Log() { }

  }
}
