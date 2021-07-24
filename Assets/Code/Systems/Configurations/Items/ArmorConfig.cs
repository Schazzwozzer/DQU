using UnityEngine;

namespace DQU.Configurations
{
    /// <summary>
    /// A data object which describes an armor, equippable by the player.
    /// </summary>
    [CreateAssetMenu( fileName = "Armor", menuName = "Configurations/Items/Armor", order = 2 )]
    public class ArmorConfig : EquippableConfig
    {
        [SerializeField]
        private int _defenseValue;
        /// <summary>
        /// The amount by which the Player's defense will be passively increased when this Armor is equipped.
        /// </summary>
        public int DefenseValue => _defenseValue;
    }
}
