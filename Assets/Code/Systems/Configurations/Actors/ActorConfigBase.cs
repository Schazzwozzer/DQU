using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DQU.Configurations
{
    /// <summary>
    /// Base class for data blueprints which describe characters.
    /// </summary>
    public abstract class ActorConfigRoot : ScriptableObject
    {
        public abstract int MaxHealth { get; }

        protected int _currentHealth;
        public int CurrentHealth => _currentHealth;

    }
}
