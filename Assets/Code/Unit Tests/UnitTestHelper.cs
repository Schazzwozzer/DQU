using System.IO;
using UnityEngine;
using UnityEditor;
using DQU.Configurations;

namespace DQU.UnitTests
{
    /// <summary>
    /// Assist with loading in assets for Unit Tests.
    /// </summary>
    public static class UnitTestHelper
    {

        private static string _configurationsPath = "Assets/Data/Configurations/";

        private static string _playerConfigPath = Path.Combine( _configurationsPath, "Tests/Test Player Config.asset" );
        private static string _testOpponentConfigPath = Path.Combine( _configurationsPath, "Tests/Test Enemy Config.asset" );

        public static PlayerConfig TestPlayerConfig
        {
            get 
            {
                var config = AssetDatabase.LoadAssetAtPath<PlayerConfig>( _playerConfigPath );
                if( config is null )
                    Debug.LogError( "Test Player configuration not loaded. Path was:\n" + _playerConfigPath );
                return config;
            }
        }

        public static TestNPCConfig TestOpponentConfig
        {
            get
            {
                var config = AssetDatabase.LoadAssetAtPath<TestNPCConfig>( _testOpponentConfigPath );
                if( config is null )
                    Debug.LogError( "Test Opponent configuration not loaded. Path was:\n" + _testOpponentConfigPath );
                return config;
            }
        }

    }
}
