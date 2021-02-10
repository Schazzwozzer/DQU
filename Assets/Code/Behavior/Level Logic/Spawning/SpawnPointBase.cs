using System.Collections;
using UnityEngine;

namespace DQU
{
    /// <summary>
    /// Marks the position at which some Prefab can be spawned.
    /// </summary>
    public abstract class SpawnPointBase : MonoBehaviour
    {
        /// <summary>Instantiate the Prefab at this Spawn Point's position.</summary>
        /// <param name="parent">The parent Transform for the new instance.</param>
        public abstract void Spawn( Transform parent );

    }
}