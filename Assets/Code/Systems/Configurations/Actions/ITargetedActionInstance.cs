namespace DQU.Configurations
{
    /// <summary>
    /// <para>A public contract that this instantiated Action has Effects
    /// which are to be directed toward an other, target Actor.</para>
    /// It may additionally have Effects which affect the originating Actor.
    /// </summary>
    public interface ITargetedActionInstance : ISelfActionInstance
    {
        public new TargetedActionConfig Configuration { get; }

        /// <summary>The Actor at whom the Action is directed—the defender, generally.</summary>
        public ActorStateBase TargetActor { get; }

        public ActionEffect[] TargetEffects { get; }
    }
}