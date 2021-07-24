using UnityEngine;
using System;

namespace DQU.Configurations
{
    /// <summary>
    /// Describes a single, specific change to an Actor's vitals 
    /// or state, typically administered as part of an Action.
    /// </summary>
    [Serializable]
    public struct ActionEffect
    {
        public enum Effect
        {
            HealthDamage,
            HealthRestore,
            FatigueDamage,
            FatigueRestore
        }

        [SerializeField]
        private Effect _effectType;
        /// <summary>
        /// Determines what this Effect does.
        /// </summary>
        public Effect EffectType => _effectType;

        [SerializeField]
        private int _magnitude;
        /// <summary>
        /// The amount of Health or Fatigue that this Effect can change.
        /// </summary>
        public int Magnitude => _magnitude;



        public ActionEffect( Effect effectType, int magnitude )
        {
            this._effectType = effectType;
            this._magnitude = magnitude;
        }


        public override string ToString()
        {
            switch( _effectType )
            {
                case Effect.HealthDamage:
                    return string.Format( "Damage {0} health", _magnitude );
                case Effect.HealthRestore:
                    return string.Format( "Restore {0} health", _magnitude );
                case Effect.FatigueDamage:
                    return string.Format( "Damage {0} fatigue", _magnitude );
                case Effect.FatigueRestore:
                    return string.Format( "Restore {0} fatigue", _magnitude );
                default:
                    return "Invalid ActionEffect";
            }
        }

    }
}
