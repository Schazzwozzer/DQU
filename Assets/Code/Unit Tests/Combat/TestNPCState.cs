using DQU.Configurations;

namespace DQU.UnitTests
{
    /// <summary>
    /// <para>—Unit testing only—</para>
    /// The runtime instantiation of a Test NPC's data, including its current health.
    /// </summary>
    public class TestNPCState : ActorStateBase
    {
        protected TestNPCConfig _configuration;

        public SelfActionConfig HealSelfConfig { get { return _configuration.HealSelfConfig; } }

        public TargetedActionConfig AttackOpponentConfig { get { return _configuration.AttackOpponentConfig; } }


        public TestNPCState( TestNPCConfig config )
        {
            this._configuration = config;

            this._maxHealth = config.MaxHealth;
            this._currentHealth = config.MaxHealth;
        }


        public SelfActionConfig GetActionConfig( int index = 0 )
        {
            return _configuration.Actions[index];
        }


        public void SetHealth( int value )
        {
            this._currentHealth = value;
        }

    }
}
