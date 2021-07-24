using System;
using UnityEngine;

namespace DQU.Configurations
{
    /// <summary>
    /// A data blueprint describing an Action taken during combat, 
    /// which attempts to apply Effects to another Actor.
    /// </summary>
    [CreateAssetMenu( fileName = "Targeted Action", menuName = "Configurations/Actions/Targeted Action", order = 2 )]
    public class TargetedActionConfig : SelfActionConfig
    {
        [SerializeField]
        protected ActionEffect[] _targetEffects;
        /// <summary>
        /// The effects that will be applied to the target Actor, should this Action land successfully.
        /// </summary>
        public ActionEffect[] TargetEffects => _targetEffects;
    }
}
