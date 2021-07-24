using System.Collections.Generic;
using UnityEngine;

namespace DQU.Configurations
{
    /// <summary>
    /// A data object which describes attributes of the player character.
    /// </summary>
    [CreateAssetMenu( fileName = "Player Config", menuName = "Configurations/Actor/Player", order = 1 )]
    public sealed class PlayerConfig : ActorConfigRoot
    {
        public string DisplayName => "Player";

        [SerializeField]
        private Attributes _attributes;
        public Attributes Attributes { get { return _attributes; } }

        private int _maxHealth;
        public override int MaxHealth => _maxHealth;

        private WeaponConfig _equippedWeapon;
        public WeaponConfig EquippedWeapon => _equippedWeapon;

        private ArmorConfig _equippedArmor;
        public ArmorConfig EquippedArmor => _equippedArmor;

        public Inventory Inventory { get; private set; }

        [SerializeField]
        private WeaponConfig _startingWeapon;

    }
}
