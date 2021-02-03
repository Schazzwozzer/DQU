using System.Collections;
using UnityEngine;
using SDD.Events;
using DQU.Events;

namespace DQU
{
    /// <summary>
    /// Marks the initial position of the Player within a Level.
    /// </summary>
    public class SpawnPlayer : SpawnPoint
    {
        [SerializeField]
        private Transform _prefab;

        /// <summary>Instantiate the Player Prefab at this Spawn Point's position.</summary>
        public override void Spawn()
        {
            Transform player = Instantiate( _prefab );

            EventManager.Instance.Raise( new PlayerSpawnedEvent( player ) );
        }

    }
}