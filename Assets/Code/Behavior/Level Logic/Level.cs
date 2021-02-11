﻿using System.Collections;
using UnityEngine;

namespace DQU
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _actorsContainer;
        [SerializeField] private Room _startingRoom;
        [SerializeField] private SpawnPlayer _playerSpawn;

        private void Start()
        {
            _playerSpawn.Spawn( _actorsContainer );

            _startingRoom.EnterRoom();
        }

    }
}