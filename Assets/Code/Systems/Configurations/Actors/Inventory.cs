using System;
using System.Collections.Generic;

namespace DQU.Configurations
{
    /// <summary>
    /// Runtime instantiation of the items in the Player's possession.
    /// </summary>
    [Serializable]
    public sealed class Inventory
    {
        // Using a SortedDictionary so that we can readily find an Item by its Config,
        // and so that the Player can sort it by weight, value, etc.
        private SortedDictionary<ItemConfig, ItemDetails> _items = new SortedDictionary<ItemConfig, ItemDetails>();
        public SortedDictionary<ItemConfig, ItemDetails> Items => _items;

        private WeaponConfig _equippedWeapon;
        public WeaponConfig EquippedWeapon => _equippedWeapon;

        private ArmorConfig _equippedArmor;
        public ArmorConfig EquippedArmor => _equippedArmor;

        private int _cumulativeWeight;
        public int Weight => _cumulativeWeight;


        /// <summary>
        /// Simple data blob describing the quantity of a 
        /// specific Item, whether or not it's equipped, etc.
        /// </summary>
        public sealed class ItemDetails
        {
            private int _count;
            /// <summary>The quantity of this item in the Inventory.</summary>
            public int Count
            {
                get { return _count; }
                set { _count = value; }
            }

            private bool _isEquipped;
            /// <summary>Does the player currently have this item equipped?</summary>
            public bool IsEquipped => _isEquipped;


            public ItemDetails( int count, bool isEquipped = false )
            {
                this._count = count;
                this._isEquipped = isEquipped;
            }
        }


        public Inventory()
        {
            _cumulativeWeight = 0;
        }


        public void AddItem( ItemConfig item )
        {
            ItemDetails details;
            if( _items.TryGetValue( item, out details ) )
            {
                // If the Inventory already has one of this item, just increment its Count by 1.
                details.Count += 1;
            }
            else
            {
                // Otherwise add a new entry for this item.
                _items.Add(
                    item,
                    new ItemDetails( 1 ) );
            }
        }


        public void RemoveItem( ItemConfig item )
        {
            ItemDetails details;
            if( _items.TryGetValue( item, out details ) )
            {
                // Decrement the item's Count by 1.
                details.Count -= 1;
                if( details.Count <= 0 )
                    // If there are no more of this item in the Inventory, 
                    // remove its entry from the Dictionary.
                    _items.Remove( item );
            }
            else
                throw new ArgumentException( "Trying to remove item from Inventory that was not present." );
        }

        /// <summary>
        /// Iterate through all of the items in the Inventory and sum up the total weight.
        /// </summary>
        private void EvaluateCumulativeWeight()
        {
            _cumulativeWeight = 0;
            foreach( KeyValuePair<ItemConfig, ItemDetails> entry in _items )
            {
                _cumulativeWeight += entry.Key.Weight * entry.Value.Count;
            }
        }


    }
}
