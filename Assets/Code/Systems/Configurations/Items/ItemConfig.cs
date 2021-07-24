using System;
using UnityEngine;
using UnityEngine.Localization;

namespace DQU.Configurations
{
    /// <summary>
    /// A data blueprint for a very basic item, which only has a name, weight, and value.
    /// </summary>
    [CreateAssetMenu( fileName = "Item Config", menuName = "Configurations/Items/Basic", order = 2 )]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField]
        protected LocalizedString _displayName;
        public LocalizedString DisplayName { get { return _displayName; } }

        [SerializeField, Min( 0 )]
        protected int _weight;
        public int Weight { get; }

        [SerializeField, Min( 0 )]
        protected int _baseValue;
        public int Value { get; }
    }
}