using UnityEngine;
using UnityEngine.Localization;
using System;

namespace DQU.Configurations
{
    /// <summary>
    /// A data blueprint which describes a non-player character.
    /// </summary>
    [CreateAssetMenu( fileName = "NPC Config", menuName = "Configurations/Actor/NPC", order = 2 )]
    public class NPCConfig : ActorConfigRoot
    {
        [SerializeField]
        protected LocalizedString _displayName;
        public LocalizedString DisplayName => _displayName;

        [Min( 0 )]
        [SerializeField] protected int _maxHealth;
        public override int MaxHealth => _maxHealth;


        [SerializeField]
        protected SelfActionConfig[] _actions;
        public SelfActionConfig[] Actions { get { return _actions; } }

    }
}