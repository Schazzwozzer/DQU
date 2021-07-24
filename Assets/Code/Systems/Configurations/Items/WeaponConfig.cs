using System;
using UnityEngine;
using UnityEngine.Localization;

namespace DQU.Configurations
{
    /// <summary>
    /// A data object which describes a weapon, equippable by the player.
    /// </summary>
    [CreateAssetMenu( fileName = "Weapon", menuName = "Configurations/Items/Weapon", order = 1 )]
    public class WeaponConfig : EquippableConfig
    {
        [SerializeField]
        private TargetedActionConfig _balancedAttack;
        public TargetedActionConfig BalancedAttack { get { return _balancedAttack; } }

        [SerializeField]
        private TargetedActionConfig _aggressiveAttack;
        public TargetedActionConfig AggressiveAttack { get { return _aggressiveAttack; } }

        [SerializeField]
        private TargetedActionConfig _defensiveAttack;
        public TargetedActionConfig DefensiveAttack { get { return _defensiveAttack; } }
    }
}
