using System;
using UnityEngine;

namespace DQU.Configurations
{
    /// <summary>
    /// A data blueprint describing an Action taken during combat, 
    /// which either has no Effect or only affects the source Actor.
    /// </summary>
    [CreateAssetMenu( fileName = "Self Action", menuName = "Configurations/Actions/Self Action", order = 1 )]
    public class SelfActionConfig : ScriptableObject
    {
        [SerializeField, Min( 1 )]
        protected int _time;
        public int Time { get { return _time; } }

        [SerializeField, Min( 0 )]
        protected int _defense;
        /// <summary>
        /// The defensive bonus that the attacker maintains while this Attack is pending.
        /// </summary>
        public int Defense => _defense;

        [SerializeField, Min( 0 )]
        protected int _fatigueCost;
        /// <summary>
        /// The amount of Fatigue the attacker must expend to perform this Attack.
        /// </summary>
        public int FatigueCost => _fatigueCost;        

        [SerializeField]
        protected ActionEffect[] _selfEffects;
        /// <summary>
        /// The effects that will be applied to the acting Actor.
        /// </summary>
        public ActionEffect[] SelfEffects => _selfEffects;

    }
}
