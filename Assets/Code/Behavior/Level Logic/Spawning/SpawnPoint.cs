using System.Collections;
using UnityEngine;

namespace DQU
{
    /// <summary>
    /// Marks the position at which some Prefab can be spawned.
    /// </summary>
    public abstract class SpawnPoint : MonoBehaviour
    {
        /// <summary>Instantiate the Prefab at this Spawn Point's position.</summary>
        public abstract void Spawn();

    }
}