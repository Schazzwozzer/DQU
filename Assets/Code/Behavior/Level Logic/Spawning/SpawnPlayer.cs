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
        public override Transform Spawn( Transform parent )
        {
            Transform player = Instantiate( _prefab, parent, false );
            player.localPosition = transform.position;
            player.rotation = Quaternion.identity;

            EventManager.Instance.Raise( new PlayerSpawnedEvent( player ) );

            return player;
        }

    }
}