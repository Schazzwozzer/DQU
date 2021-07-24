namespace DQU.Configurations
{
    /// <summary>
    /// A singular instance of an Action, which attempts to apply Effects to another Actor.
    /// <para>Derived from a <see cref="SelfActionConfig"/>.</para>
    /// </summary>
    public class SelfActionInstance : ISelfActionInstance
    {
        public SelfActionConfig Configuration { get; private set; }

        private ActionTargetAndEffects _sourceTargetAndEffects;

        /// <summary>The Actor from whom the Action originates—the attacker, generally.</summary>
        public ActorStateBase SourceActor { get { return _sourceTargetAndEffects.Target; } }

        /// <summary>The Effects to be applied to the originating Actor.</summary>
        public ActionEffect[] SourceEffects { get { return _sourceTargetAndEffects.Effects; } }


        public SelfActionInstance(
            SelfActionConfig config,
            ActorStateBase source )
        {
            this.Configuration = config;
            this._sourceTargetAndEffects = new ActionTargetAndEffects(
                source,
                config.SelfEffects );
        }


        public void ApplyEffectsToSelf()
        {
            _sourceTargetAndEffects.ApplyEffects();
        }

    }
}
