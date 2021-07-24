namespace DQU.Configurations
{
    /// <summary>
    /// A singular instance of an Action, which attempts to apply Effects to another Actor.
    /// <para>It may additionally have Effects which affect the originating Actor.</para>
    /// </summary>
    public class TargetedActionInstance : ITargetedActionInstance
    {
        public TargetedActionConfig Configuration { get; private set; }
        SelfActionConfig ISelfActionInstance.Configuration => Configuration;

        /** Self Target and Effects **/
        private ActionTargetAndEffects _sourceTargetAndEffects;

        /// <summary>The Actor from whom the Action originates—the attacker, generally.</summary>
        public ActorStateBase SourceActor { get { return _sourceTargetAndEffects.Target; } }

        public ActionEffect[] SourceEffects { get { return _sourceTargetAndEffects.Effects; } }

        /** Other Target and Effects **/
        private ActionTargetAndEffects _otherTargetAndEffects;

        /// <summary>The Actor at whom the Action is directed—the defender, generally.</summary>
        public ActorStateBase TargetActor { get { return _otherTargetAndEffects.Target; } }

        /// <summary>The Effects to be applied to the target, other Actor.</summary>
        public ActionEffect[] TargetEffects { get { return _otherTargetAndEffects.Effects; } }


        public TargetedActionInstance(
            TargetedActionConfig config,
            ActorStateBase source,
            ActorStateBase target )
        {
            this.Configuration = config;
            this._sourceTargetAndEffects = new ActionTargetAndEffects( source, config.SelfEffects );
            this._otherTargetAndEffects = new ActionTargetAndEffects( target, config.TargetEffects );
        }


        public void ApplyEffectsToTarget()
        {
            _otherTargetAndEffects.ApplyEffects();
        }

    }
}
