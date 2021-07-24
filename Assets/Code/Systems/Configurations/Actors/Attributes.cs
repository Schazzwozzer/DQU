using UnityEngine;
using System;

namespace DQU.Configurations
{
    /// <summary>
    /// A data object describing the Six Classic RPG Attributes — or at least Dunjonquest's take on them.
    /// </summary>
    [Serializable]
    public class Attributes
    {
        [SerializeField, Min( 0 )]
        protected int strength;
        public int Strength { get { return strength; } }

        [SerializeField, Min( 0 )]
        protected int constitution;
        public int Constitution { get { return constitution; } }

        [SerializeField, Min( 0 )]
        protected int dexterity;
        public int Dexterity { get { return dexterity; } }

        [SerializeField, Min( 0 )]
        protected int intelligence;
        public int Intelligence { get { return intelligence; } }

        [SerializeField, Min( 0 )]
        protected int intuition;
        public int Intuition { get { return intuition; } }

        [SerializeField, Min( 0 )]
        protected int ego;
        public int Ego { get { return ego; } }
    }
}
