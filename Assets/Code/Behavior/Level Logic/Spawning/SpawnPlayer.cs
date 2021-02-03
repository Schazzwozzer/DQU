using System.Collections;
using UnityEngine;
using SDD.Events;
using DQU.Events;

namespace DQU
{
    /// <summary>
    /// Marks the initial position of the Player within a Level.
    /// </summary>
    public class SpawnPlayer : SpawnPointBase
    {
        [SerializeField]
        private Transform _prefab;

        /// <summary>Instantiate the Player Prefab at this Spawn Point's position.</summary>
        /// <param name="parent">The parent Transform for the new Player instance.</param>
        public override void Spawn( Transform parent )
        {
            Transform player = Instantiate( _prefab, transform.position, Quaternion.identity, parent );

            EventManager.Instance.Raise( new PlayerSpawnedEvent( player ) );
        }

    }
}