namespace DQU.Configurations
{
    /// <summary>
    /// A public contract that this is an instantiated Action, originating from a specific Actor.
    /// <para>It may optionally have Effects which are applied to that origin Actor (self).</para>
    /// </summary>
    public interface ISelfActionInstance
    {
        public SelfActionConfig Configuration { get; }

        /// <summary>The Actor from whom the attack originates—the attacker.</summary>
        public ActorStateBase SourceActor { get; }

        public ActionEffect[] SourceEffects { get; }
    }
}