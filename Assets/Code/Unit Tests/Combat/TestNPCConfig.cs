using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DQU.Configurations;

namespace DQU.UnitTests
{
    /// <summary>
    /// <para>—Unit testing only—</para>
    /// A data blueprint which describes a test non-player character.
    /// </summary>
    [CreateAssetMenu( fileName = "NPC Config (Unit Testing)", menuName = "Configurations/Unit Testing/NPC", order = 2 )]
    public class TestNPCConfig : NPCConfig
    {
        [SerializeField]
        private TargetedActionConfig _attackOpponentConfig;
        public TargetedActionConfig AttackOpponentConfig { get { return _attackOpponentConfig; } }

        [SerializeField]
        private SelfActionConfig _healSelfConfig;
        public SelfActionConfig HealSelfConfig { get { return _healSelfConfig; } }

    }
}