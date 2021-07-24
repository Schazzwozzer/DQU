

namespace DQU.Configurations
{
    /// <summary>
    /// The runtime instantiation of an NPC's data, 
    /// including its current health.
    /// </summary>
    public class NPCState : ActorStateBase
    {
        protected NPCConfig _configuration;


        public NPCState( NPCConfig config )
        {
            this._configuration = config;

            this._maxHealth = config.MaxHealth;
            this._currentHealth = config.MaxHealth;
        }


        public SelfActionConfig GetActionConfig( int index = 0 )
        {
            return _configuration.Actions[index];
        }

    }
}
