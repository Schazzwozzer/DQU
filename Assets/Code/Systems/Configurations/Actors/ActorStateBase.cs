using System;

namespace DQU.Configurations
{
    /// <summary>
    /// The runtime instantiation of an Actor's data, depicting 
    /// a singular instance of that Actor and its current health.
    /// </summary>
    public abstract class ActorStateBase
    {
        /****    VITALS    ****/
        protected int _maxHealth;
        public int MaxHealth => _maxHealth;

        protected int _currentHealth;
        public int CurrentHealth => _currentHealth;


        public void ApplyEffect( ActionEffect effect )
        {
            switch( effect.EffectType )
            {
                case ActionEffect.Effect.HealthDamage:
                    ReduceHealth( effect.Magnitude );
                    break;
                case ActionEffect.Effect.HealthRestore:
                    IncreaseHealth( effect.Magnitude );
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Reduce the Actor's current health by the specified value.
        /// </summary>
        protected void IncreaseHealth( int change )
        {
            _currentHealth = Math.Min( _maxHealth, _currentHealth + change );
        }

        /// <summary>
        /// Reduce the Actor's current health by the specified value.
        /// </summary>
        protected void ReduceHealth( int change )
        {
            _currentHealth = Math.Max( 0, _currentHealth - change );

            if( _currentHealth == 0 )
            {
                // Die!
            }
        }

    }
}