using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using DQU.Combat;
using DQU.Configurations;

namespace DQU.UnitTests
{
    // Unit tests for confirming that basic functions of the Combat Engine are working as intended.
    public class CombatTests
    {
        private TestNPCState _opponent1;
        private TestNPCState _opponent2;

        private TestNPCConfig _opponentConfig;

        #region Setup

        [SetUp]
        public void SetUp()
        {
            if( _opponentConfig is null )
                _opponentConfig = UnitTestHelper.TestOpponentConfig;

            _opponent1 = new TestNPCState( _opponentConfig );
            _opponent2 = new TestNPCState( _opponentConfig );
        }

        #endregion


        [TearDown]
        public void TearDown()
        {
            CombatEngine.Reset();

            _opponent1 = null;
            _opponent2 = null;
        }


        [Test]
        public void ActionsAddedToTimeline()
        {
            var attack = new TargetedActionInstance(
                _opponent1.AttackOpponentConfig, _opponent1, _opponent2 );

            CombatEngine.BeginAction( attack );

            Assert.That( CombatEngine.HasPendingActions, Is.True );
        }


        [Test]
        public void ActionsReduceHealthOfOther()
        {
            int startHealth = _opponent1.CurrentHealth;

            var attack = new TargetedActionInstance(
                _opponent2.AttackOpponentConfig,
                _opponent2,
                _opponent1 );

            CombatEngine.BeginAction( attack );

            while( CombatEngine.HasPendingActions )
                CombatEngine.Tick();

            Assert.That( _opponent1.CurrentHealth, Is.LessThan( startHealth ));
        }

        [Test]
        public void ActionsRestoreHealthToSelf()
        {
            _opponent1.SetHealth( 1 );

            var selfHeal = new SelfActionInstance( _opponent1.HealSelfConfig, _opponent1 );
            CombatEngine.BeginAction( selfHeal );

            while( CombatEngine.HasPendingActions )
                CombatEngine.Tick();

            Assert.That( _opponent1.CurrentHealth, Is.GreaterThan( 1 ));
        }


        [Test]
        public void FinishedActionsAreRemovedFromTimeline()
        {
            var action = new SelfActionInstance( _opponent1.HealSelfConfig, _opponent1 );
            CombatEngine.BeginAction( action );

            int i = 0, j = 0;       // j is to prevent infinite loops
            while( i < action.Configuration.Time + 1 && !(j > 100) )
            {
                if( CombatEngine.Tick() )
                    ++i;
                ++j;
            }
            Assert.That( !CombatEngine.HasPendingActions );
        }

    }
}
