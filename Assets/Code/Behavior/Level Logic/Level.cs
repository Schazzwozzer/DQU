using System.Collections;
using UnityEngine;

namespace DQU
{
    public class Level : MonoBehaviour
    {

        [SerializeField]
        private SpawnPlayer _playerSpawn;

        private void Start()
        {
            _playerSpawn.Spawn();
        }

    }
}