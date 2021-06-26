using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace DQU
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _actorsContainer;
        [SerializeField] private Room _startingRoom;
        [SerializeField] private SpawnPlayer _playerSpawn;
        [SerializeField] private Light2D _devLight;
        [SerializeField] private Transform _levelLightsParent;
        [SerializeField] private PlayerLight _playerLight;
        

        private void Start()
        {
            ProcessLights();

            Transform player = _playerSpawn.Spawn( _actorsContainer );
            _playerLight.Configure( player );

            _startingRoom.EnterRoom();
        }


        private void ProcessLights()
        {
            // For development purposes, scenes may have a full, global light.
            // Make sure that's turned off when the game runs.
            GameObject.Destroy( _devLight.gameObject );

            // Similarly, make sure the Level's designer-placed lights are on.
            _levelLightsParent.gameObject.SetActive( true );
        }

    }
}