using System;
using UnityEngine;

namespace DQU.Configurations
{
    /// <summary>
    /// The runtime instantiation of the Player's data, 
    /// including character attributes, inventory, and equipped items.
    /// </summary>
    [Serializable]
    public sealed class PlayerState : ActorStateBase
    {
        private Attributes _attributes;
        public Attributes Attributes { get { return _attributes; } }

        /****    VITALS    ****/
        private int _maxFatigue;
        public int MaxFatigue => _maxFatigue;

        private int _currentFatigue;
        public int CurrentFatigue => _currentFatigue;

        /****    INVENTORY AND EQUIPMENT    ****/
        private Inventory _inventory;
        public Inventory Inventory => _inventory;

        public WeaponConfig EquippedWeapon => _inventory.EquippedWeapon;
        public ArmorConfig EquippedArmor => _inventory.EquippedArmor;


        public PlayerState( PlayerConfig config )
        {
            this._attributes = config.Attributes;
            this._maxHealth = config.MaxHealth;
            this._currentHealth = config.MaxHealth;
            this._inventory = config.Inventory;
        }


    }
}
